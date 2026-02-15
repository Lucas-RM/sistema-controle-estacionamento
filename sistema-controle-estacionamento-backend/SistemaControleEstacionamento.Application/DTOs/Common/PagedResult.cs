namespace SistemaControleEstacionamento.Application.DTOs.Common;

/// <summary>
/// Resultado paginado de uma consulta
/// </summary>
/// <typeparam name="T">Tipo dos itens retornados</typeparam>
public class PagedResult<T>
{
    /// <summary>
    /// Lista de itens da página atual
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Metadados de paginação
    /// </summary>
    public PaginationMetadata Pagination { get; set; } = null!;
}
