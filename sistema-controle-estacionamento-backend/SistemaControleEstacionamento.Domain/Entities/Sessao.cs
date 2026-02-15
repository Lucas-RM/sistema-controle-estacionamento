namespace SistemaControleEstacionamento.Domain.Entities;

public class Sessao
{
    public Guid Id { get; set; }
    public Guid VeiculoId { get; set; }
    public DateTimeOffset DataHoraEntrada { get; set; }
    public DateTimeOffset? DataHoraSaida { get; set; }
    public decimal? ValorCobrado { get; set; }
    public bool Ativa { get; set; }
    public string RowVersion { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // Navegação
    public virtual Veiculo Veiculo { get; set; } = null!;
}

