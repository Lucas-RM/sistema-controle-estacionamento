using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Application.DTOs.Veiculo;

public class VeiculoDto
{
    public Guid Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string? Modelo { get; set; }
    public string? Cor { get; set; }
    public TipoVeiculo Tipo { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

