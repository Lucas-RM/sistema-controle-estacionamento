namespace SistemaControleEstacionamento.Application.Interfaces;

public interface IPrecificacaoService
{
    decimal CalcularValor(DateTimeOffset dataHoraEntrada, DateTimeOffset dataHoraSaida);
}

