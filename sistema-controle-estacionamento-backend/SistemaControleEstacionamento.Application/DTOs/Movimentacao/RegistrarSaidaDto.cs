namespace SistemaControleEstacionamento.Application.DTOs.Movimentacao;

public class RegistrarSaidaDto
{
    public Guid SessaoId { get; set; }
    public string RowVersion { get; set; } = string.Empty;
}

