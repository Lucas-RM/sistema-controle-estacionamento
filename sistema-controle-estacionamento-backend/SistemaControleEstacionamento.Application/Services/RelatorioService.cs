using SistemaControleEstacionamento.Application.DTOs.Relatorios;
using SistemaControleEstacionamento.Application.Interfaces;
using SistemaControleEstacionamento.Domain.Enums;
using SistemaControleEstacionamento.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaControleEstacionamento.Application.Services;

public class RelatorioService : IRelatorioService
{
    private readonly ISessaoRepository _sessaoRepository;

    public RelatorioService(ISessaoRepository sessaoRepository)
    {
        _sessaoRepository = sessaoRepository;
    }

    public async Task<IEnumerable<FaturamentoPorDiaDto>> GetFaturamentoPorDiaAsync(PeriodoRelatorio periodo)
    {
        var (dataInicio, dataFim) = CalcularPeriodo(periodo);

        var sessoes = await _sessaoRepository.GetSessoesFinalizadasAsync(dataInicio, dataFim);

        var resultado = sessoes
            .Where(s => s.DataHoraSaida.HasValue && s.ValorCobrado.HasValue)
            .GroupBy(s => s.DataHoraSaida!.Value.Date)
            .Select(g => new FaturamentoPorDiaDto
            {
                Data = g.Key.Date,
                TotalSessoes = g.Count(),
                ValorTotal = g.Sum(s => s.ValorCobrado!.Value)
            })
            .OrderByDescending(r => r.Data)
            .ToList();

        return resultado;
    }

    public async Task<IEnumerable<TopVeiculoDto>> GetTopVeiculosAsync(DateTime? dataInicio, DateTime? dataFim)
    {
        var inicio = dataInicio.HasValue ? new DateTimeOffset(dataInicio.Value, TimeSpan.Zero) : DateTimeOffset.UtcNow.AddDays(-30);
        var fim = dataFim.HasValue ? new DateTimeOffset(dataFim.Value, TimeSpan.Zero) : DateTimeOffset.UtcNow;

        var sessoes = await _sessaoRepository.GetSessoesFinalizadasAsync(inicio, fim);

        var resultado = sessoes
            .Where(s => s.DataHoraSaida.HasValue)
            .GroupBy(s => s.Veiculo)
            .Select(g => new TopVeiculoDto
            {
                Placa = g.Key.Placa,
                Modelo = g.Key.Modelo,
                TempoTotalMinutos = g.Sum(s =>
                    (s.DataHoraSaida!.Value - s.DataHoraEntrada).TotalMinutes),
                QuantidadeSessoes = g.Count()
            })
            .OrderByDescending(v => v.TempoTotalMinutos)
            .Take(10)
            .ToList();

        return resultado;
    }

    public async Task<IEnumerable<OcupacaoPorHoraDto>> GetOcupacaoPorHoraAsync(DateTime dataInicio, DateTime dataFim)
    {
        var inicioOffset = new DateTimeOffset(dataInicio, TimeSpan.Zero);
        var fimOffset = new DateTimeOffset(dataFim, TimeSpan.Zero);
        
        var sessoes = await _sessaoRepository.GetSessoesByPeriodoAsync(inicioOffset, fimOffset);

        var resultado = new List<OcupacaoPorHoraDto>();

        var horaAtual = new DateTimeOffset(
            inicioOffset.Year, 
            inicioOffset.Month, 
            inicioOffset.Day, 
            inicioOffset.Hour, 
            0, 
            0, 
            TimeSpan.Zero);

        while (horaAtual < fimOffset)
        {
            var horaInicio = horaAtual < inicioOffset ? inicioOffset : horaAtual;
            var proximaHora = horaAtual.AddHours(1);
            var horaFim = proximaHora > fimOffset ? fimOffset : proximaHora;

            var quantidade = sessoes.Count(s =>
                s.DataHoraEntrada <= horaFim &&
                (s.DataHoraSaida == null || s.DataHoraSaida >= horaInicio));

            var periodo = $"{horaInicio:yyyy-MM-ddTHH:mm:ss} - {horaFim.AddSeconds(-1):yyyy-MM-ddTHH:mm:ss}";

            resultado.Add(new OcupacaoPorHoraDto
            {
                Periodo = periodo,
                Hora = horaAtual.Hour,
                QuantidadeVeiculos = quantidade
            });

            horaAtual = proximaHora;
        }

        return resultado.OrderBy(r => r.Periodo);
    }

    private (DateTimeOffset dataInicio, DateTimeOffset dataFim) CalcularPeriodo(PeriodoRelatorio periodo)
    {
        var hoje = DateTimeOffset.UtcNow.Date;
        var fimPeriodo = hoje.AddTicks(-1);
        
        return periodo switch
        {
            PeriodoRelatorio.Ultimos7Dias => (
                hoje.AddDays(-7),
                fimPeriodo
            ),
            PeriodoRelatorio.Ultimos30Dias => (
                hoje.AddDays(-30),
                fimPeriodo
            ),
            _ => (
                hoje.AddDays(-7),
                fimPeriodo
            )
        };
    }
}

