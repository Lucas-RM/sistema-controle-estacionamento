using SistemaControleEstacionamento.Domain.Entities;

namespace SistemaControleEstacionamento.Domain.Interfaces;

public interface ISessaoRepository : IRepository<Sessao>
{
    Task<Sessao?> GetSessaoAtivaByVeiculoIdAsync(Guid veiculoId);
    Task<IEnumerable<Sessao>> GetSessoesAtivasAsync();
    Task<IEnumerable<Sessao>> GetSessoesFinalizadasAsync(DateTimeOffset dataInicio, DateTimeOffset dataFim);
    Task<IEnumerable<Sessao>> GetSessoesByPeriodoAsync(DateTimeOffset dataInicio, DateTimeOffset dataFim);
    Task<IEnumerable<Sessao>> GetSessoesByVeiculoIdAsync(Guid veiculoId);
}

