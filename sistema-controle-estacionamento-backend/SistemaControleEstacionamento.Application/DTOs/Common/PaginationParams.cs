using System.ComponentModel.DataAnnotations;

namespace SistemaControleEstacionamento.Application.DTOs.Common;

public class PaginationParams
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public string SortOrder { get; set; } = "desc";
}
