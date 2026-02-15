namespace SistemaControleEstacionamento.Application.DTOs.Movimentacao;

/// <summary>
/// Dados para registro de entrada de veículo
/// </summary>
public class RegistrarEntradaDto
{
    /// <summary>
    /// ID do veículo que está entrando (obrigatório)
    /// </summary>
    /// <remarks>
    /// O veículo deve estar previamente cadastrado.
    /// Não pode ter sessão ativa.
    /// </remarks>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid VeiculoId { get; set; }
}

