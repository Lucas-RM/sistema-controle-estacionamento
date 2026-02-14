using SistemaControleEstacionamento.Application.Interfaces;

namespace SistemaControleEstacionamento.Application.Services;

public class PrecificacaoService : IPrecificacaoService
{
    private const decimal ValorPrimeiraHora = 5.00m;
    private const decimal ValorHoraAdicional = 3.00m;

    public decimal CalcularValor(DateTimeOffset dataHoraEntrada, DateTimeOffset dataHoraSaida)
    {
        if (dataHoraSaida <= dataHoraEntrada)
            return 0;

        var tempoTotal = dataHoraSaida - dataHoraEntrada;
        var horasTotais = (decimal)tempoTotal.TotalHours;

        // Primeira hora ou fração
        if (horasTotais <= 1)
            return ValorPrimeiraHora;

        // Primeira hora + horas adicionais (arredondado para cima)
        var horasAdicionais = Math.Ceiling(horasTotais - 1);
        return ValorPrimeiraHora + (horasAdicionais * ValorHoraAdicional);
    }
}

