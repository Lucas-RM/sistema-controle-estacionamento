using Microsoft.EntityFrameworkCore;
using SistemaControleEstacionamento.Application.DTOs.Common;
using SistemaControleEstacionamento.Domain.Entities;

namespace SistemaControleEstacionamento.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query, 
        int page, 
        int pageSize)
    {
        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Data = items,
            Pagination = PaginationMetadata.Create(page, pageSize, totalItems)
        };
    }

    public static IQueryable<Sessao> ApplySorting(
        this IQueryable<Sessao> query, 
        string? sortBy, 
        string sortOrder)
    {
        var isDescending = sortOrder.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "datahoraentrada" => isDescending 
                ? query.OrderByDescending(s => s.DataHoraEntrada) 
                : query.OrderBy(s => s.DataHoraEntrada),
            "datahorasaida" => isDescending 
                ? query.OrderByDescending(s => s.DataHoraSaida) 
                : query.OrderBy(s => s.DataHoraSaida),
            "valorcobrado" => isDescending 
                ? query.OrderByDescending(s => s.ValorCobrado) 
                : query.OrderBy(s => s.ValorCobrado),
            "placa" => isDescending 
                ? query.OrderByDescending(s => s.Veiculo.Placa) 
                : query.OrderBy(s => s.Veiculo.Placa),
            _ => query.OrderByDescending(s => s.DataHoraEntrada)
        };
    }

    public static IQueryable<Veiculo> ApplySorting(
        this IQueryable<Veiculo> query, 
        string? sortBy, 
        string sortOrder)
    {
        var isDescending = sortOrder.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "placa" => isDescending 
                ? query.OrderByDescending(v => v.Placa) 
                : query.OrderBy(v => v.Placa),
            "modelo" => isDescending 
                ? query.OrderByDescending(v => v.Modelo) 
                : query.OrderBy(v => v.Modelo),
            "cor" => isDescending 
                ? query.OrderByDescending(v => v.Cor) 
                : query.OrderBy(v => v.Cor),
            "tipo" => isDescending 
                ? query.OrderByDescending(v => v.Tipo) 
                : query.OrderBy(v => v.Tipo),
            _ => query.OrderBy(v => v.Placa)
        };
    }
}
