using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Relatorios;
using SistemaControleEstacionamento.Application.Interfaces;
using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet("faturamento")]
    public async Task<ActionResult<IEnumerable<FaturamentoPorDiaDto>>> GetFaturamento(
        [FromQuery] PeriodoRelatorio periodo = PeriodoRelatorio.Ultimos7Dias)
    {
        var resultado = await _relatorioService.GetFaturamentoPorDiaAsync(periodo);
        return Ok(resultado);
    }

    [HttpGet("top-veiculos")]
    public async Task<ActionResult<IEnumerable<TopVeiculoDto>>> GetTopVeiculos(
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null)
    {
        var resultado = await _relatorioService.GetTopVeiculosAsync(dataInicio, dataFim);
        return Ok(resultado);
    }

    [HttpGet("ocupacao-hora")]
    public async Task<ActionResult<IEnumerable<OcupacaoPorHoraDto>>> GetOcupacaoPorHora(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        if (dataFim < dataInicio)
            return BadRequest(new { message = "A data fim deve ser posterior à data início." });

        var resultado = await _relatorioService.GetOcupacaoPorHoraAsync(dataInicio, dataFim);
        return Ok(resultado);
    }
}

