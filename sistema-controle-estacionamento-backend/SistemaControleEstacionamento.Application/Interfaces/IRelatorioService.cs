using SistemaControleEstacionamento.Application.DTOs.Relatorios;
using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Application.Interfaces;

public interface IRelatorioService
{
    Task<IEnumerable<FaturamentoPorDiaDto>> GetFaturamentoPorDiaAsync(PeriodoRelatorio periodo);
    Task<IEnumerable<TopVeiculoDto>> GetTopVeiculosAsync(DateTime? dataInicio, DateTime? dataFim);
    Task<IEnumerable<OcupacaoPorHoraDto>> GetOcupacaoPorHoraAsync(DateTime dataInicio, DateTime dataFim);
}

