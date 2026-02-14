using Microsoft.EntityFrameworkCore;
using SistemaControleEstacionamento.Domain.Entities;
using SistemaControleEstacionamento.Domain.Interfaces;
using SistemaControleEstacionamento.Infrastructure.Data;

namespace SistemaControleEstacionamento.Infrastructure.Repositories;

public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
{
    public VeiculoRepository(EstacionamentoDbContext context)
        : base(context)
    {
    }

    public async Task<Veiculo?> GetByPlacaAsync(string placa)
    {
        return await _dbSet
            .FirstOrDefaultAsync(v => v.Placa == placa);
    }

    public async Task<bool> ExistsByPlacaAsync(string placa)
    {
        return await _dbSet
            .AnyAsync(v => v.Placa == placa);
    }

    public async Task<bool> HasSessaoAtivaAsync(Guid veiculoId)
    {
        return await _context.Sessoes
            .AnyAsync(s => s.VeiculoId == veiculoId && s.Ativa);
    }
}

