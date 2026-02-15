using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Common;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;
using SistemaControleEstacionamento.Application.Interfaces;

namespace SistemaControleEstacionamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimentacaoController : ControllerBase
{
    private readonly IMovimentacaoService _movimentacaoService;

    public MovimentacaoController(IMovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    [HttpPost("entrada")]
    public async Task<ActionResult<SessaoDto>> RegistrarEntrada([FromBody] RegistrarEntradaDto dto)
    {
        var sessao = await _movimentacaoService.RegistrarEntradaAsync(dto);
        return Created($"/api/movimentacao/{sessao.Id}", sessao);
    }

    [HttpPost("saida")]
    public async Task<ActionResult<SessaoDto>> RegistrarSaida([FromBody] RegistrarSaidaDto dto)
    {
        var sessao = await _movimentacaoService.RegistrarSaidaAsync(dto);
        return Ok(sessao);
    }

    [HttpGet("calcular-valor/{sessaoId}")]
    public async Task<ActionResult<decimal>> CalcularValor(Guid sessaoId)
    {
        var valor = await _movimentacaoService.CalcularValorAsync(sessaoId);
        return Ok(new { valor });
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<SessaoDto>>> ListarSessoes([FromQuery] SessaoQueryParams queryParams)
    {
        var sessoes = await _movimentacaoService.ListarSessoesAsync(queryParams);
        return Ok(sessoes);
    }
}

