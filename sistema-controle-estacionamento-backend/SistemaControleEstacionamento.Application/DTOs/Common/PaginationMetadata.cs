namespace SistemaControleEstacionamento.Application.DTOs.Common;

/// <summary>
/// Metadados de paginação
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Número da página atual
    /// </summary>
    /// <example>1</example>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Quantidade de itens por página
    /// </summary>
    /// <example>10</example>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de itens em todas as páginas
    /// </summary>
    /// <example>150</example>
    public int TotalItems { get; set; }

    /// <summary>
    /// Total de páginas disponíveis
    /// </summary>
    /// <example>15</example>
    public int TotalPages { get; set; }

    /// <summary>
    /// Indica se existe próxima página
    /// </summary>
    /// <example>true</example>
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Indica se existe página anterior
    /// </summary>
    /// <example>false</example>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Cria metadados de paginação
    /// </summary>
    public static PaginationMetadata Create(int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        return new PaginationMetadata
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNextPage = currentPage < totalPages,
            HasPreviousPage = currentPage > 1
        };
    }
}
