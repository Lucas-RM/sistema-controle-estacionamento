using SistemaControleEstacionamento.Application.DTOs.Veiculo;

namespace SistemaControleEstacionamento.Application.Interfaces;

public interface IVeiculoService
{
    Task<VeiculoDto> CreateAsync(CreateVeiculoDto dto);
    Task<VeiculoDto> UpdateAsync(Guid id, UpdateVeiculoDto dto);
    Task<IEnumerable<VeiculoDto>> GetAllAsync();
    Task<VeiculoDto?> GetByIdAsync(Guid id);
    Task<VeiculoDto?> GetByPlacaAsync(string placa);
}

