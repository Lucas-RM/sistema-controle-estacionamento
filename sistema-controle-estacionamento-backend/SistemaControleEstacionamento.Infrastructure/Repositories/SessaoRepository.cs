using Microsoft.EntityFrameworkCore;
using SistemaControleEstacionamento.Domain.Entities;
using SistemaControleEstacionamento.Domain.Interfaces;
using SistemaControleEstacionamento.Infrastructure.Data;

namespace SistemaControleEstacionamento.Infrastructure.Repositories;

public class SessaoRepository : Repository<Sessao>, ISessaoRepository
{
    public SessaoRepository(EstacionamentoDbContext context)
        : base(context)
    {
    }

    public async Task<Sessao?> GetSessaoAtivaByVeiculoIdAsync(Guid veiculoId)
    {
        return await _dbSet
            .Include(s => s.Veiculo)
            .FirstOrDefaultAsync(s => s.VeiculoId == veiculoId && s.Ativa);
    }

    public async Task<IEnumerable<Sessao>> GetSessoesAtivasAsync()
    {
        return await _dbSet
            .Include(s => s.Veiculo)
            .Where(s => s.Ativa)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sessao>> GetSessoesFinalizadasAsync(DateTimeOffset dataInicio, DateTimeOffset dataFim)
    {
        return await _dbSet
            .Include(s => s.Veiculo)
            .Where(s => s.DataHoraSaida.HasValue &&
                       s.DataHoraSaida.Value >= dataInicio &&
                       s.DataHoraSaida.Value <= dataFim)
            .ToListAsync();
    }

    public async Task<IEnumerable<Sessao>> GetSessoesByPeriodoAsync(DateTimeOffset dataInicio, DateTimeOffset dataFim)
    {
        return await _dbSet
            .Include(s => s.Veiculo)
            .Where(s => s.DataHoraEntrada <= dataFim &&
                       (s.DataHoraSaida == null || s.DataHoraSaida >= dataInicio))
            .ToListAsync();
    }

    public async Task<IEnumerable<Sessao>> GetSessoesByVeiculoIdAsync(Guid veiculoId)
    {
        return await _dbSet
            .Include(s => s.Veiculo)
            .Where(s => s.VeiculoId == veiculoId)
            .ToListAsync();
    }

    public override async Task<Sessao?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(s => s.Veiculo)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}

