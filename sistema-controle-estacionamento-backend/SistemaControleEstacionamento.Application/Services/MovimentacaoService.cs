using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;
using SistemaControleEstacionamento.Application.Interfaces;
using SistemaControleEstacionamento.Domain.Exceptions;
using SistemaControleEstacionamento.Domain.Interfaces;

namespace SistemaControleEstacionamento.Application.Services;

public class MovimentacaoService : IMovimentacaoService
{
    private readonly ISessaoRepository _sessaoRepository;
    private readonly IVeiculoRepository _veiculoRepository;
    private readonly IPrecificacaoService _precificacaoService;
    private readonly IMapper _mapper;

    public MovimentacaoService(
        ISessaoRepository sessaoRepository,
        IVeiculoRepository veiculoRepository,
        IPrecificacaoService precificacaoService,
        IMapper mapper)
    {
        _sessaoRepository = sessaoRepository;
        _veiculoRepository = veiculoRepository;
        _precificacaoService = precificacaoService;
        _mapper = mapper;
    }

    public async Task<SessaoDto> RegistrarEntradaAsync(RegistrarEntradaDto dto)
    {
        var veiculo = await _veiculoRepository.GetByIdAsync(dto.VeiculoId);
        if (veiculo == null)
            throw new NotFoundException("Veículo não encontrado.");

        // Verificar se já existe sessão ativa (idempotência)
        var sessaoAtiva = await _sessaoRepository.GetSessaoAtivaByVeiculoIdAsync(dto.VeiculoId);
        if (sessaoAtiva != null)
            return _mapper.Map<SessaoDto>(sessaoAtiva);

        var sessao = new Domain.Entities.Sessao
        {
            Id = Guid.NewGuid(),
            VeiculoId = dto.VeiculoId,
            DataHoraEntrada = DateTimeOffset.UtcNow,
            Ativa = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        try
        {
            var sessaoCriada = await _sessaoRepository.AddAsync(sessao);
            return _mapper.Map<SessaoDto>(sessaoCriada);
        }
        catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
        {
            sessaoAtiva = await _sessaoRepository.GetSessaoAtivaByVeiculoIdAsync(dto.VeiculoId);
            if (sessaoAtiva != null)
                return _mapper.Map<SessaoDto>(sessaoAtiva);
            
            throw;
        }
    }

    public async Task<SessaoDto> RegistrarSaidaAsync(RegistrarSaidaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RowVersion))
            throw new BusinessException("O campo RowVersion é obrigatório para fechar a sessão.");

        var sessao = await _sessaoRepository.GetByIdAsync(dto.SessaoId);
        if (sessao == null)
            throw new NotFoundException("Sessão não encontrada.");

        if (!sessao.Ativa)
            throw new BusinessException("Não é possível fechar uma sessão que já foi encerrada.");

        // Validar RowVersion
        var rowVersionBytes = Convert.FromBase64String(dto.RowVersion);
        if (!sessao.RowVersion.SequenceEqual(rowVersionBytes))
            throw new ConcurrencyException("A sessão foi modificada por outro usuário. Busque os dados atualizados e tente novamente.");

        var dataHoraSaida = DateTimeOffset.UtcNow;
        if (dataHoraSaida < sessao.DataHoraEntrada)
            throw new BusinessException("A data/hora de saída não pode ser anterior à entrada.");

        sessao.DataHoraSaida = dataHoraSaida;
        sessao.ValorCobrado = _precificacaoService.CalcularValor(sessao.DataHoraEntrada, dataHoraSaida);
        sessao.Ativa = false;
        sessao.UpdatedAt = DateTimeOffset.UtcNow;

        try
        {
            await _sessaoRepository.UpdateAsync(sessao);
            return _mapper.Map<SessaoDto>(sessao);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException("A sessão foi modificada por outro usuário. Busque os dados atualizados e tente novamente.");
        }
    }

    public async Task<decimal> CalcularValorAsync(Guid sessaoId)
    {
        var sessao = await _sessaoRepository.GetByIdAsync(sessaoId);
        if (sessao == null)
            throw new NotFoundException("Sessão não encontrada.");

        if (!sessao.Ativa)
            throw new BusinessException("A sessão já foi finalizada.");

        var dataHoraSaida = DateTimeOffset.UtcNow;
        return _precificacaoService.CalcularValor(sessao.DataHoraEntrada, dataHoraSaida);
    }

    public async Task<IEnumerable<SessaoDto>> GetSessoesAtivasAsync(string? placa = null)
    {
        var sessoes = await _sessaoRepository.GetSessoesAtivasAsync();

        if (!string.IsNullOrWhiteSpace(placa))
        {
            var placaNormalizada = placa.Trim().ToUpper().Replace("-", "").Replace(" ", "");
            sessoes = sessoes.Where(s => s.Veiculo.Placa == placaNormalizada);
        }

        return _mapper.Map<IEnumerable<SessaoDto>>(sessoes);
    }
}

