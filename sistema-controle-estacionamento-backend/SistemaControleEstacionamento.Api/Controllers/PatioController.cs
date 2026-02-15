using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Common;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;
using SistemaControleEstacionamento.Application.Interfaces;

namespace SistemaControleEstacionamento.Api.Controllers;

/// <summary>
/// Consulta de veículos atualmente no pátio (sessões ativas)
/// </summary>
/// <remarks>
/// Este controller fornece uma visão em tempo real dos veículos estacionados.
/// É um atalho para listar sessões com status=ativas.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PatioController : ControllerBase
{
    private readonly IMovimentacaoService _movimentacaoService;

    public PatioController(IMovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    /// <summary>
    /// Lista todos os veículos atualmente no pátio
    /// </summary>
    /// <param name="queryParams">Parâmetros de consulta (paginação, filtros)</param>
    /// <returns>Lista paginada de sessões ativas</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/patio/agora?placa=ABC&amp;page=1&amp;pageSize=10
    /// 
    /// Comportamento:
    /// - Retorna apenas sessões ativas (Ativa = true)
    /// - Força status=ativas automaticamente
    /// - Suporta filtro por placa
    /// - Suporta paginação e ordenação
    /// 
    /// Casos de uso:
    /// - Monitoramento em tempo real do pátio
    /// - Verificação de ocupação atual
    /// - Busca rápida de veículo no pátio
    /// </remarks>
    /// <response code="200">Lista de veículos no pátio retornada com sucesso</response>
    [HttpGet("agora")]
    [ProducesResponseType(typeof(PagedResult<SessaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<SessaoDto>>> GetVeiculosNoPatio([FromQuery] SessaoQueryParams queryParams)
    {
        queryParams.Status = "ativas";
        var sessoes = await _movimentacaoService.ListarSessoesAsync(queryParams);
        return Ok(sessoes);
    }
}

