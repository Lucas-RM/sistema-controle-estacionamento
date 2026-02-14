using AutoMapper;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;
using SistemaControleEstacionamento.Application.Interfaces;
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
        // Validar se veículo existe
        var veiculo = await _veiculoRepository.GetByIdAsync(dto.VeiculoId);
        if (veiculo == null)
            throw new KeyNotFoundException("Veículo não encontrado.");

        // Validar se não há sessão ativa
        var sessaoAtiva = await _sessaoRepository.GetSessaoAtivaByVeiculoIdAsync(dto.VeiculoId);
        if (sessaoAtiva != null)
            throw new InvalidOperationException("O veículo já possui uma sessão ativa.");

        // Criar nova sessão
        var sessao = new Domain.Entities.Sessao
        {
            Id = Guid.NewGuid(),
            VeiculoId = dto.VeiculoId,
            DataHoraEntrada = DateTimeOffset.UtcNow,
            Ativa = true,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var sessaoCriada = await _sessaoRepository.AddAsync(sessao);
        return _mapper.Map<SessaoDto>(sessaoCriada);
    }

    public async Task<SessaoDto> RegistrarSaidaAsync(RegistrarSaidaDto dto)
    {
        var sessao = await _sessaoRepository.GetByIdAsync(dto.SessaoId);
        if (sessao == null)
            throw new KeyNotFoundException("Sessão não encontrada.");

        if (!sessao.Ativa)
            throw new InvalidOperationException("A sessão já foi finalizada.");

        var dataHoraSaida = DateTimeOffset.UtcNow;
        if (dataHoraSaida < sessao.DataHoraEntrada)
            throw new InvalidOperationException("A data/hora de saída não pode ser anterior à entrada.");

        sessao.DataHoraSaida = dataHoraSaida;
        sessao.ValorCobrado = _precificacaoService.CalcularValor(sessao.DataHoraEntrada, dataHoraSaida);
        sessao.Ativa = false;
        sessao.UpdatedAt = DateTimeOffset.UtcNow;

        await _sessaoRepository.UpdateAsync(sessao);
        return _mapper.Map<SessaoDto>(sessao);
    }

    public async Task<decimal> CalcularValorAsync(Guid sessaoId)
    {
        var sessao = await _sessaoRepository.GetByIdAsync(sessaoId);
        if (sessao == null)
            throw new KeyNotFoundException("Sessão não encontrada.");

        if (!sessao.Ativa)
            throw new InvalidOperationException("A sessão já foi finalizada.");

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

