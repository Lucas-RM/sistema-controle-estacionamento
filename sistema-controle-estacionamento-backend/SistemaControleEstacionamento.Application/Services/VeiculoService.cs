using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaControleEstacionamento.Application.DTOs.Common;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;
using SistemaControleEstacionamento.Application.Extensions;
using SistemaControleEstacionamento.Application.Interfaces;
using SistemaControleEstacionamento.Domain.Enums;
using SistemaControleEstacionamento.Domain.Interfaces;

namespace SistemaControleEstacionamento.Application.Services;

public class VeiculoService : IVeiculoService
{
    private readonly IVeiculoRepository _repository;
    private readonly IMapper _mapper;

    public VeiculoService(IVeiculoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<VeiculoDto> CreateAsync(CreateVeiculoDto dto)
    {
        var placaNormalizada = NormalizarPlaca(dto.Placa);

        if (await _repository.ExistsByPlacaAsync(placaNormalizada))
            throw new InvalidOperationException("Já existe um veículo cadastrado com esta placa.");

        var veiculo = _mapper.Map<Domain.Entities.Veiculo>(dto);
        veiculo.Placa = placaNormalizada;
        veiculo.CreatedAt = DateTimeOffset.UtcNow;

        var veiculoCriado = await _repository.AddAsync(veiculo);
        return _mapper.Map<VeiculoDto>(veiculoCriado);
    }

    public async Task<VeiculoDto> UpdateAsync(Guid id, UpdateVeiculoDto dto)
    {
        var veiculo = await _repository.GetByIdAsync(id);
        if (veiculo == null)
            throw new KeyNotFoundException("Veículo não encontrado.");

        if (dto.Modelo != null)
            veiculo.Modelo = dto.Modelo;
        if (dto.Cor != null)
            veiculo.Cor = dto.Cor;
        if (dto.Tipo.HasValue)
            veiculo.Tipo = dto.Tipo.Value;

        veiculo.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(veiculo);
        return _mapper.Map<VeiculoDto>(veiculo);
    }

    public async Task<IEnumerable<VeiculoDto>> GetAllAsync()
    {
        var veiculos = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<VeiculoDto>>(veiculos);
    }

    public async Task<VeiculoDto?> GetByIdAsync(Guid id)
    {
        var veiculo = await _repository.GetByIdAsync(id);
        return veiculo == null ? null : _mapper.Map<VeiculoDto>(veiculo);
    }

    public async Task<VeiculoDto?> GetByPlacaAsync(string placa)
    {
        var placaNormalizada = NormalizarPlaca(placa);
        var veiculo = await _repository.GetByPlacaAsync(placaNormalizada);
        return veiculo == null ? null : _mapper.Map<VeiculoDto>(veiculo);
    }

    public async Task<PagedResult<VeiculoDto>> ListarVeiculosAsync(VeiculoQueryParams queryParams)
    {
        var query = _repository.GetQueryable();

        // Filtro de placa
        if (!string.IsNullOrWhiteSpace(queryParams.Placa))
        {
            var placaNormalizada = NormalizarPlaca(queryParams.Placa);
            query = query.Where(v => v.Placa.Contains(placaNormalizada));
        }

        // Filtro de modelo
        if (!string.IsNullOrWhiteSpace(queryParams.Modelo))
        {
            var modeloNormalizado = queryParams.Modelo.Trim().ToUpper();
            query = query.Where(v => v.Modelo != null && v.Modelo.ToUpper().Contains(modeloNormalizado));
        }

        // Filtro de cor
        if (!string.IsNullOrWhiteSpace(queryParams.Cor))
        {
            var corNormalizada = queryParams.Cor.Trim().ToUpper();
            query = query.Where(v => v.Cor != null && v.Cor.ToUpper().Contains(corNormalizada));
        }

        // Filtro de tipo
        if (!string.IsNullOrWhiteSpace(queryParams.Tipo))
        {
            if (Enum.TryParse<TipoVeiculo>(queryParams.Tipo, out var tipo))
                query = query.Where(v => v.Tipo == tipo);
        }

        // Filtro de sessão ativa
        if (queryParams.ComSessaoAtiva.HasValue)
        {
            if (queryParams.ComSessaoAtiva.Value)
                query = query.Where(v => v.Sessoes.Any(s => s.Ativa));
            else
                query = query.Where(v => !v.Sessoes.Any(s => s.Ativa));
        }

        // Ordenação
        query = query.ApplySorting(queryParams.SortBy, queryParams.SortOrder);

        query = query.AsNoTracking();

        // Paginação
        var pagedVeiculos = await query.ToPagedResultAsync(queryParams.Page, queryParams.PageSize);

        var veiculosDto = _mapper.Map<IEnumerable<VeiculoDto>>(pagedVeiculos.Data);

        return new PagedResult<VeiculoDto>
        {
            Data = veiculosDto,
            Pagination = pagedVeiculos.Pagination
        };
    }

    private string NormalizarPlaca(string placa)
    {
        return placa.Trim().ToUpper().Replace("-", "").Replace(" ", "");
    }
}

