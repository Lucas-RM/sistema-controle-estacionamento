using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Application.DTOs.Veiculo;

public class CreateVeiculoDto
{
    public string Placa { get; set; } = string.Empty;
    public string? Modelo { get; set; }
    public string? Cor { get; set; }
    public TipoVeiculo Tipo { get; set; }
}

