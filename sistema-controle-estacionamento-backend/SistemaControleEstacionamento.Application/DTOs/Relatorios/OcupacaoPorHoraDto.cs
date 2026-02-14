namespace SistemaControleEstacionamento.Application.DTOs.Relatorios;

public class OcupacaoPorHoraDto
{
    public string Periodo { get; set; } = string.Empty;
    public int Hora { get; set; }
    public int QuantidadeVeiculos { get; set; }
}

