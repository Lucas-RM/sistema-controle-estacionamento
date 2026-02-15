using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Common;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;
using SistemaControleEstacionamento.Application.Interfaces;

namespace SistemaControleEstacionamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatioController : ControllerBase
{
    private readonly IMovimentacaoService _movimentacaoService;

    public PatioController(IMovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    [HttpGet("agora")]
    public async Task<ActionResult<PagedResult<SessaoDto>>> GetVeiculosNoPatio([FromQuery] SessaoQueryParams queryParams)
    {
        queryParams.Status = "ativas";
        var sessoes = await _movimentacaoService.ListarSessoesAsync(queryParams);
        return Ok(sessoes);
    }
}

