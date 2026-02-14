namespace SistemaControleEstacionamento.Application.DTOs.Relatorios;

public class TopVeiculoDto
{
    public string Placa { get; set; } = string.Empty;
    public string? Modelo { get; set; }
    public double TempoTotalMinutos { get; set; }
    public int QuantidadeSessoes { get; set; }
}

