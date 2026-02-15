using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Relatorios;
using SistemaControleEstacionamento.Application.Interfaces;
using SistemaControleEstacionamento.Domain.Enums;

namespace SistemaControleEstacionamento.Api.Controllers;

/// <summary>
/// Relatórios gerenciais e estatísticas do estacionamento
/// </summary>
/// <remarks>
/// Este controller fornece dados analíticos para gestão do negócio:
/// - Faturamento por período
/// - Veículos mais frequentes
/// - Ocupação por horário
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RelatoriosController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatoriosController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    /// <summary>
    /// Obtém faturamento diário por período
    /// </summary>
    /// <param name="periodo">Período do relatório (1: Últimos 7 dias, 2: Últimos 30 dias)</param>
    /// <returns>Lista de faturamento agrupado por dia</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/relatorios/faturamento?periodo=1
    /// 
    /// Períodos disponíveis:
    /// - 1: Ultimos7Dias
    /// - 2: Ultimos30Dias
    /// 
    /// Resposta contém:
    /// - Data do dia
    /// - Total de sessões finalizadas
    /// - Valor total arrecadado
    /// 
    /// Ordenado por data (mais recente primeiro)
    /// </remarks>
    /// <response code="200">Relatório gerado com sucesso</response>
    [HttpGet("faturamento")]
    [ProducesResponseType(typeof(IEnumerable<FaturamentoPorDiaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FaturamentoPorDiaDto>>> GetFaturamento(
        [FromQuery] PeriodoRelatorio periodo = PeriodoRelatorio.Ultimos7Dias)
    {
        var resultado = await _relatorioService.GetFaturamentoPorDiaAsync(periodo);
        return Ok(resultado);
    }

    /// <summary>
    /// Obtém top 10 veículos mais frequentes
    /// </summary>
    /// <param name="dataInicio">Data inicial do período (opcional, padrão: 30 dias atrás)</param>
    /// <param name="dataFim">Data final do período (opcional, padrão: hoje)</param>
    /// <returns>Lista dos 10 veículos com maior tempo total de permanência</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/relatorios/top-veiculos?dataInicio=2025-01-01&amp;dataFim=2025-12-31
    /// 
    /// Comportamento:
    /// - Considera apenas sessões finalizadas
    /// - Ordena por tempo total de permanência (decrescente)
    /// - Retorna no máximo 10 veículos
    /// 
    /// Resposta contém:
    /// - Placa e modelo do veículo
    /// - Tempo total em minutos
    /// - Quantidade de sessões
    /// 
    /// Formato de data: yyyy-MM-dd ou ISO 8601
    /// </remarks>
    /// <response code="200">Relatório gerado com sucesso</response>
    [HttpGet("top-veiculos")]
    [ProducesResponseType(typeof(IEnumerable<TopVeiculoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TopVeiculoDto>>> GetTopVeiculos(
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null)
    {
        var resultado = await _relatorioService.GetTopVeiculosAsync(dataInicio, dataFim);
        return Ok(resultado);
    }

    /// <summary>
    /// Obtém ocupação do pátio por hora
    /// </summary>
    /// <param name="dataInicio">Data/hora inicial do período (obrigatório)</param>
    /// <param name="dataFim">Data/hora final do período (obrigatório)</param>
    /// <returns>Lista de ocupação agrupada por hora</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/relatorios/ocupacao-hora?dataInicio=2025-01-15T00:00:00&amp;dataFim=2025-01-15T23:59:59
    /// 
    /// Comportamento:
    /// - Agrupa sessões por hora
    /// - Conta veículos presentes em cada hora
    /// - Considera sessões ativas e finalizadas no período
    /// 
    /// Resposta contém:
    /// - Período (intervalo de 1 hora)
    /// - Hora (0-23)
    /// - Quantidade de veículos
    /// 
    /// Casos de uso:
    /// - Análise de horários de pico
    /// - Planejamento de capacidade
    /// - Otimização de recursos
    /// </remarks>
    /// <response code="200">Relatório gerado com sucesso</response>
    /// <response code="400">Data fim anterior à data início</response>
    [HttpGet("ocupacao-hora")]
    [ProducesResponseType(typeof(IEnumerable<OcupacaoPorHoraDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
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

