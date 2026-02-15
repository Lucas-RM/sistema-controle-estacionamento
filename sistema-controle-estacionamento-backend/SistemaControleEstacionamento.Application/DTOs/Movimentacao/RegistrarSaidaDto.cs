namespace SistemaControleEstacionamento.Application.DTOs.Movimentacao;

/// <summary>
/// Dados para registro de saída de veículo
/// </summary>
public class RegistrarSaidaDto
{
    /// <summary>
    /// ID da sessão a ser fechada (obrigatório)
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid SessaoId { get; set; }

    /// <summary>
    /// Versão da sessão para controle de concorrência otimista (obrigatório)
    /// </summary>
    /// <remarks>
    /// Este campo é essencial para evitar conflitos em ambientes multi-usuário.
    /// Deve ser obtido da resposta de GET ou POST entrada.
    /// Se o RowVersion não corresponder, a operação falhará com 409 Conflict.
    /// </remarks>
    /// <example>a1b2c3d4-e5f6-7890-abcd-ef1234567890</example>
    public string RowVersion { get; set; } = string.Empty;
}

