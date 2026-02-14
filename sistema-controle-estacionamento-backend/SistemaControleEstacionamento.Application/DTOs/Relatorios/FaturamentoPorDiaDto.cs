namespace SistemaControleEstacionamento.Application.DTOs.Relatorios;

public class FaturamentoPorDiaDto
{
    public DateTime Data { get; set; }
    public int TotalSessoes { get; set; }
    public decimal ValorTotal { get; set; }
}

