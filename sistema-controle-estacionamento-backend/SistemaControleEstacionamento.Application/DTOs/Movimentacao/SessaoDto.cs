using SistemaControleEstacionamento.Application.DTOs.Veiculo;

namespace SistemaControleEstacionamento.Application.DTOs.Movimentacao;

/// <summary>
/// Dados de uma sessão de estacionamento
/// </summary>
public class SessaoDto
{
    /// <summary>
    /// Identificador único da sessão
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// ID do veículo associado
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid VeiculoId { get; set; }

    /// <summary>
    /// Dados completos do veículo
    /// </summary>
    public VeiculoDto Veiculo { get; set; } = null!;

    /// <summary>
    /// Data/hora de entrada no estacionamento (UTC, ISO 8601)
    /// </summary>
    /// <example>2025-01-15T14:30:00Z</example>
    public DateTimeOffset DataHoraEntrada { get; set; }

    /// <summary>
    /// Data/hora de saída do estacionamento (UTC, ISO 8601)
    /// </summary>
    /// <remarks>
    /// Null enquanto a sessão estiver ativa.
    /// Preenchido automaticamente ao fechar a sessão.
    /// </remarks>
    /// <example>2025-01-15T16:45:00Z</example>
    public DateTimeOffset? DataHoraSaida { get; set; }

    /// <summary>
    /// Valor cobrado em reais
    /// </summary>
    /// <remarks>
    /// Null enquanto a sessão estiver ativa.
    /// Calculado automaticamente ao fechar a sessão.
    /// Primeira hora: R$ 5,00
    /// Horas adicionais: R$ 3,00 cada (arredondado para cima)
    /// </remarks>
    /// <example>11.00</example>
    public decimal? ValorCobrado { get; set; }

    /// <summary>
    /// Indica se a sessão está ativa (veículo no pátio)
    /// </summary>
    /// <example>true</example>
    public bool Ativa { get; set; }

    /// <summary>
    /// Versão da sessão para controle de concorrência otimista
    /// </summary>
    /// <remarks>
    /// Deve ser enviado ao fechar a sessão.
    /// Atualizado automaticamente a cada modificação.
    /// </remarks>
    /// <example>a1b2c3d4-e5f6-7890-abcd-ef1234567890</example>
    public string RowVersion { get; set; } = string.Empty;

    /// <summary>
    /// Tempo de permanência no estacionamento
    /// </summary>
    /// <remarks>
    /// Calculado automaticamente:
    /// - Sessão ativa: tempo desde entrada até agora
    /// - Sessão fechada: tempo entre entrada e saída
    /// </remarks>
    /// <example>02:15:30</example>
    public TimeSpan? TempoPermanencia { get; set; }
}

