using SistemaControleEstacionamento.Domain.Entities;

namespace SistemaControleEstacionamento.Domain.Interfaces;

public interface IVeiculoRepository : IRepository<Veiculo>
{
    Task<Veiculo?> GetByPlacaAsync(string placa);
    Task<bool> ExistsByPlacaAsync(string placa);
    Task<bool> HasSessaoAtivaAsync(Guid veiculoId);
}

