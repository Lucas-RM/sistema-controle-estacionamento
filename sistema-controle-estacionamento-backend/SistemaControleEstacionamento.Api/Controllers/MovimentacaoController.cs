using Microsoft.AspNetCore.Mvc;
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
        try
        {
            var sessao = await _movimentacaoService.RegistrarEntradaAsync(dto);
            return Created($"/api/movimentacao/{sessao.Id}", sessao);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("saida")]
    public async Task<ActionResult<SessaoDto>> RegistrarSaida([FromBody] RegistrarSaidaDto dto)
    {
        try
        {
            var sessao = await _movimentacaoService.RegistrarSaidaAsync(dto);
            return Ok(sessao);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("calcular-valor/{sessaoId}")]
    public async Task<ActionResult<decimal>> CalcularValor(Guid sessaoId)
    {
        try
        {
            var valor = await _movimentacaoService.CalcularValorAsync(sessaoId);
            return Ok(new { valor });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}

