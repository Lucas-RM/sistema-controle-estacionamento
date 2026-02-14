using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Application.DTOs.Relatorios;

public class PeriodoRelatorioDto
{
    public PeriodoRelatorio Periodo { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}

