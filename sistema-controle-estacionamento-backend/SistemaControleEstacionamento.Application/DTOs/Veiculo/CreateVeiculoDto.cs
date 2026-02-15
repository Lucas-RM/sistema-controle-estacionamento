using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Application.DTOs.Veiculo;

/// <summary>
/// Dados para cadastro de um novo veículo
/// </summary>
public class CreateVeiculoDto
{
    /// <summary>
    /// Placa do veículo (obrigatório)
    /// </summary>
    /// <remarks>
    /// Formatos aceitos:
    /// - Brasil antigo: AAA9999 (ex: ABC1234)
    /// - Brasil Mercosul: AAA9A99 (ex: ABC1D23)
    /// - Argentina novo: AA999AA (ex: AB123CD)
    /// - Argentina antigo: AAA999 (ex: ABC123)
    /// 
    /// A placa é normalizada automaticamente (maiúsculas, sem espaços/hífens).
    /// </remarks>
    /// <example>ABC1234</example>
    public string Placa { get; set; } = string.Empty;

    /// <summary>
    /// Modelo do veículo (opcional)
    /// </summary>
    /// <example>Honda Civic</example>
    public string? Modelo { get; set; }

    /// <summary>
    /// Cor do veículo (opcional)
    /// </summary>
    /// <example>Preto</example>
    public string? Cor { get; set; }

    /// <summary>
    /// Tipo do veículo (obrigatório)
    /// </summary>
    /// <remarks>
    /// Valores possíveis:
    /// - 1: Carro
    /// - 2: Moto
    /// - 3: Caminhonete
    /// </remarks>
    /// <example>1</example>
    public TipoVeiculo Tipo { get; set; }
}

