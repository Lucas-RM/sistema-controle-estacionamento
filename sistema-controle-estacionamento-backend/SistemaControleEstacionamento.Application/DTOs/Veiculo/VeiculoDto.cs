using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Application.DTOs.Veiculo;

/// <summary>
/// Dados de um veículo cadastrado
/// </summary>
public class VeiculoDto
{
    /// <summary>
    /// Identificador único do veículo
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Placa do veículo (normalizada)
    /// </summary>
    /// <example>ABC1234</example>
    public string Placa { get; set; } = string.Empty;

    /// <summary>
    /// Modelo do veículo
    /// </summary>
    /// <example>Honda Civic</example>
    public string? Modelo { get; set; }

    /// <summary>
    /// Cor do veículo
    /// </summary>
    /// <example>Preto</example>
    public string? Cor { get; set; }

    /// <summary>
    /// Tipo do veículo (1: Carro, 2: Moto, 3: Caminhonete)
    /// </summary>
    /// <example>1</example>
    public TipoVeiculo Tipo { get; set; }

    /// <summary>
    /// Data/hora de cadastro (UTC, ISO 8601)
    /// </summary>
    /// <example>2025-01-15T14:30:00Z</example>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Data/hora da última atualização (UTC, ISO 8601)
    /// </summary>
    /// <example>2025-01-16T10:15:00Z</example>
    public DateTimeOffset? UpdatedAt { get; set; }
}

