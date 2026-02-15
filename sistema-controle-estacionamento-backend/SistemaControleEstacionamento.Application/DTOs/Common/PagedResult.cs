namespace SistemaControleEstacionamento.Application.DTOs.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public PaginationMetadata Pagination { get; set; } = null!;
}
