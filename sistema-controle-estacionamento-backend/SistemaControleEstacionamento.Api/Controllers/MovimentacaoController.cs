using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Common;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;
using SistemaControleEstacionamento.Application.Interfaces;

namespace SistemaControleEstacionamento.Api.Controllers;

/// <summary>
/// Gerenciamento de movimentações (entradas e saídas) de veículos
/// </summary>
/// <remarks>
/// Este controller gerencia o ciclo de vida das sessões de estacionamento:
/// - Registro de entrada (abertura de sessão)
/// - Cálculo de valor em tempo real
/// - Registro de saída (fechamento de sessão)
/// - Consulta de sessões com filtros avançados
/// 
/// IMPORTANTE:
/// - Todas as datas são em UTC (ISO 8601)
/// - Utiliza concorrência otimista (RowVersion)
/// - Entrada é idempotente (múltiplas chamadas retornam mesma sessão)
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class MovimentacaoController : ControllerBase
{
    private readonly IMovimentacaoService _movimentacaoService;

    public MovimentacaoController(IMovimentacaoService movimentacaoService)
    {
        _movimentacaoService = movimentacaoService;
    }

    /// <summary>
    /// Registra a entrada de um veículo no estacionamento
    /// </summary>
    /// <param name="dto">ID do veículo que está entrando</param>
    /// <returns>Sessão criada ou sessão ativa existente</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     POST /api/movimentacao/entrada
    ///     {
    ///         "veiculoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    ///     }
    /// 
    /// Comportamento:
    /// - Cria uma nova sessão ativa para o veículo
    /// - DataHoraEntrada é definida automaticamente (UTC)
    /// - Gera RowVersion para controle de concorrência
    /// 
    /// IDEMPOTÊNCIA:
    /// - Se o veículo já possui sessão ativa, retorna a sessão existente
    /// - Evita duplicação em caso de retry ou requisições simultâneas
    /// - Garantido por índice único no banco (VeiculoId + Ativa)
    /// 
    /// Regras de negócio:
    /// - Veículo deve estar cadastrado
    /// - Veículo não pode ter sessão ativa
    /// </remarks>
    /// <response code="201">Sessão criada com sucesso (nova entrada)</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Veículo não encontrado</response>
    [HttpPost("entrada")]
    [ProducesResponseType(typeof(SessaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SessaoDto>> RegistrarEntrada([FromBody] RegistrarEntradaDto dto)
    {
        var sessao = await _movimentacaoService.RegistrarEntradaAsync(dto);
        return Created($"/api/movimentacao/{sessao.Id}", sessao);
    }

    /// <summary>
    /// Registra a saída de um veículo do estacionamento
    /// </summary>
    /// <param name="dto">ID da sessão e RowVersion para controle de concorrência</param>
    /// <returns>Sessão fechada com valor cobrado</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     POST /api/movimentacao/saida
    ///     {
    ///         "sessaoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    ///         "rowVersion": "a1b2c3d4-e5f6-7890-abcd-ef1234567890"
    ///     }
    /// 
    /// Comportamento:
    /// - Define DataHoraSaida automaticamente (UTC)
    /// - Calcula ValorCobrado baseado no tempo de permanência
    /// - Marca sessão como inativa (Ativa = false)
    /// - Atualiza RowVersion (nova versão)
    /// 
    /// CONCORRÊNCIA OTIMISTA:
    /// - RowVersion é OBRIGATÓRIO
    /// - Se RowVersion não corresponder, retorna 409 Conflict
    /// - Cliente deve buscar dados atualizados e tentar novamente
    /// - Previne conflitos em ambientes multi-usuário
    /// 
    /// Regras de precificação:
    /// - Primeira hora ou fração: R$ 5,00
    /// - Horas adicionais: R$ 3,00 cada (arredondado para cima)
    /// - Exemplo: 2h30min = R$ 5,00 + (2 * R$ 3,00) = R$ 11,00
    /// 
    /// Regras de negócio:
    /// - Sessão deve existir
    /// - Sessão deve estar ativa
    /// - RowVersion deve ser válido
    /// </remarks>
    /// <response code="200">Sessão fechada com sucesso</response>
    /// <response code="404">Sessão não encontrada</response>
    /// <response code="409">Conflito de concorrência (RowVersion inválido ou sessão modificada)</response>
    /// <response code="422">Sessão já foi encerrada ou RowVersion não informado</response>
    [HttpPost("saida")]
    [ProducesResponseType(typeof(SessaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(object), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<SessaoDto>> RegistrarSaida([FromBody] RegistrarSaidaDto dto)
    {
        var sessao = await _movimentacaoService.RegistrarSaidaAsync(dto);
        return Ok(sessao);
    }

    /// <summary>
    /// Calcula o valor a ser cobrado de uma sessão ativa em tempo real
    /// </summary>
    /// <param name="sessaoId">ID da sessão (GUID)</param>
    /// <returns>Valor calculado em reais</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/movimentacao/calcular-valor/3fa85f64-5717-4562-b3fc-2c963f66afa6
    /// 
    /// Comportamento:
    /// - Calcula valor baseado no tempo desde a entrada até o momento atual
    /// - Não modifica a sessão (apenas consulta)
    /// - Útil para exibir valor antes de fechar a sessão
    /// 
    /// Regras de precificação:
    /// - Primeira hora ou fração: R$ 5,00
    /// - Horas adicionais: R$ 3,00 cada (arredondado para cima)
    /// </remarks>
    /// <response code="200">Valor calculado com sucesso</response>
    /// <response code="404">Sessão não encontrada</response>
    /// <response code="422">Sessão já foi finalizada</response>
    [HttpGet("calcular-valor/{sessaoId}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(object), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<decimal>> CalcularValor([FromRoute] Guid sessaoId)
    {
        var valor = await _movimentacaoService.CalcularValorAsync(sessaoId);
        return Ok(new { valor });
    }

    /// <summary>
    /// Lista sessões com paginação, filtros e ordenação
    /// </summary>
    /// <param name="queryParams">Parâmetros de consulta (filtros, paginação, ordenação)</param>
    /// <returns>Lista paginada de sessões</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/movimentacao?status=ativas&amp;placa=ABC&amp;dataInicio=2025-01-01T00:00:00Z&amp;dataFim=2025-12-31T23:59:59Z&amp;page=1&amp;pageSize=10&amp;sortBy=dataHoraEntrada&amp;sortOrder=desc
    /// 
    /// Parâmetros de filtro:
    /// - status: todas, ativas ou fechadas (padrão: todas)
    /// - placa: Busca parcial na placa do veículo
    /// - veiculoId: Filtro por ID do veículo (GUID)
    /// - dataInicio: Data/hora inicial (ISO 8601 UTC)
    /// - dataFim: Data/hora final (ISO 8601 UTC)
    /// 
    /// Parâmetros de paginação:
    /// - page: Número da página (mínimo: 1, padrão: 1)
    /// - pageSize: Itens por página (mínimo: 1, máximo: 100, padrão: 10)
    /// 
    /// Parâmetros de ordenação:
    /// - sortBy: dataHoraEntrada, dataHoraSaida, valorCobrado, placa (padrão: dataHoraEntrada)
    /// - sortOrder: asc ou desc (padrão: desc)
    /// 
    /// Formato de data:
    /// - ISO 8601 em UTC: 2025-01-15T14:30:00Z
    /// - Sempre use sufixo 'Z' para indicar UTC
    /// </remarks>
    /// <response code="200">Lista de sessões retornada com sucesso</response>
    /// <response code="400">Parâmetros de consulta inválidos</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SessaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<SessaoDto>>> ListarSessoes([FromQuery] SessaoQueryParams queryParams)
    {
        var sessoes = await _movimentacaoService.ListarSessoesAsync(queryParams);
        return Ok(sessoes);
    }
}

