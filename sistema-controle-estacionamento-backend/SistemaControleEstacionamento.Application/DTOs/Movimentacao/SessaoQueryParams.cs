using SistemaControleEstacionamento.Application.DTOs.Common;

namespace SistemaControleEstacionamento.Application.DTOs.Movimentacao;

public class SessaoQueryParams : PaginationParams
{
    public string Status { get; set; } = "todas";
    public string? Placa { get; set; }
    public Guid? VeiculoId { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
}
