using SistemaControleEstacionamento.Application.DTOs.Common;

namespace SistemaControleEstacionamento.Application.DTOs.Veiculo;

public class VeiculoQueryParams : PaginationParams
{
    public string? Placa { get; set; }
    public string? Modelo { get; set; }
    public string? Cor { get; set; }
    public string? Tipo { get; set; }
    public bool? ComSessaoAtiva { get; set; }
}
