using SistemaControleEstacionamento.Application.DTOs.Veiculo;

namespace SistemaControleEstacionamento.Application.DTOs.Movimentacao;

public class SessaoDto
{
    public Guid Id { get; set; }
    public Guid VeiculoId { get; set; }
    public VeiculoDto Veiculo { get; set; } = null!;
    public DateTimeOffset DataHoraEntrada { get; set; }
    public DateTimeOffset? DataHoraSaida { get; set; }
    public decimal? ValorCobrado { get; set; }
    public bool Ativa { get; set; }
    public string RowVersion { get; set; } = string.Empty;
    public TimeSpan? TempoPermanencia { get; set; }
}

