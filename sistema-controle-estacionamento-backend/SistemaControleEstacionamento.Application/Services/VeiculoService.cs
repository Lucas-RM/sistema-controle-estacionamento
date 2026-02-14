using AutoMapper;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;
using SistemaControleEstacionamento.Application.Interfaces;
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

    private string NormalizarPlaca(string placa)
    {
        return placa.Trim().ToUpper().Replace("-", "").Replace(" ", "");
    }
}

