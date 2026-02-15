using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Common;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;
using SistemaControleEstacionamento.Application.Interfaces;

namespace SistemaControleEstacionamento.Api.Controllers;

/// <summary>
/// Gerenciamento de veículos cadastrados no sistema
/// </summary>
/// <remarks>
/// Este controller permite o cadastro, consulta e atualização de veículos.
/// Suporta filtros avançados, paginação e ordenação.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class VeiculosController : ControllerBase
{
    private readonly IVeiculoService _veiculoService;

    public VeiculosController(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    /// <summary>
    /// Cadastra um novo veículo no sistema
    /// </summary>
    /// <param name="dto">Dados do veículo a ser cadastrado</param>
    /// <returns>Veículo cadastrado com ID gerado</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     POST /api/veiculos
    ///     {
    ///         "placa": "ABC1234",
    ///         "modelo": "Honda Civic",
    ///         "cor": "Preto",
    ///         "tipo": 1
    ///     }
    /// 
    /// Tipos de veículo:
    /// - 1: Carro
    /// - 2: Moto
    /// - 3: Caminhonete
    /// 
    /// Formatos de placa aceitos:
    /// - Brasil antigo: AAA9999 (ex: ABC1234)
    /// - Brasil Mercosul: AAA9A99 (ex: ABC1D23)
    /// - Argentina novo: AA999AA (ex: AB123CD)
    /// - Argentina antigo: AAA999 (ex: ABC123)
    /// 
    /// A placa é normalizada automaticamente (maiúsculas, sem espaços/hífens).
    /// </remarks>
    /// <response code="201">Veículo cadastrado com sucesso</response>
    /// <response code="400">Dados inválidos (validação FluentValidation) ou placa já cadastrada</response>
    [HttpPost]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VeiculoDto>> Create([FromBody] CreateVeiculoDto dto)
    {
        try
        {
            var veiculo = await _veiculoService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, veiculo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza dados de um veículo existente
    /// </summary>
    /// <param name="id">ID do veículo (GUID)</param>
    /// <param name="dto">Dados a serem atualizados (campos opcionais)</param>
    /// <returns>Veículo atualizado</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     PUT /api/veiculos/{id}
    ///     {
    ///         "modelo": "Honda Civic 2024",
    ///         "cor": "Prata",
    ///         "tipo": 1
    ///     }
    /// 
    /// Observações:
    /// - A placa NÃO pode ser alterada
    /// - Todos os campos são opcionais
    /// - Apenas os campos informados serão atualizados
    /// - UpdatedAt é atualizado automaticamente (UTC)
    /// </remarks>
    /// <response code="200">Veículo atualizado com sucesso</response>
    /// <response code="404">Veículo não encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeiculoDto>> Update([FromRoute] Guid id, [FromBody] UpdateVeiculoDto dto)
    {
        try
        {
            var veiculo = await _veiculoService.UpdateAsync(id, dto);
            return Ok(veiculo);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Lista veículos com paginação, filtros e ordenação
    /// </summary>
    /// <param name="queryParams">Parâmetros de consulta (filtros, paginação, ordenação)</param>
    /// <returns>Lista paginada de veículos</returns>
    /// <remarks>
    /// Exemplo de requisição:
    /// 
    ///     GET /api/veiculos?placa=ABC&amp;tipo=Carro&amp;comSessaoAtiva=false&amp;page=1&amp;pageSize=10&amp;sortBy=placa&amp;sortOrder=asc
    /// 
    /// Parâmetros de filtro:
    /// - placa: Busca parcial (case-insensitive)
    /// - modelo: Busca parcial (case-insensitive)
    /// - cor: Busca parcial (case-insensitive)
    /// - tipo: Carro, Moto ou Caminhonete
    /// - comSessaoAtiva: true/false (veículos no pátio)
    /// 
    /// Parâmetros de paginação:
    /// - page: Número da página (mínimo: 1, padrão: 1)
    /// - pageSize: Itens por página (mínimo: 1, máximo: 100, padrão: 10)
    /// 
    /// Parâmetros de ordenação:
    /// - sortBy: placa, modelo, cor, tipo (padrão: placa)
    /// - sortOrder: asc ou desc (padrão: asc)
    /// </remarks>
    /// <response code="200">Lista de veículos retornada com sucesso</response>
    /// <response code="400">Parâmetros de consulta inválidos</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<VeiculoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagedResult<VeiculoDto>>> GetAll([FromQuery] VeiculoQueryParams queryParams)
    {
        var veiculos = await _veiculoService.ListarVeiculosAsync(queryParams);
        return Ok(veiculos);
    }

    /// <summary>
    /// Busca um veículo por ID
    /// </summary>
    /// <param name="id">ID do veículo (GUID)</param>
    /// <returns>Dados do veículo</returns>
    /// <response code="200">Veículo encontrado</response>
    /// <response code="404">Veículo não encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeiculoDto>> GetById([FromRoute] Guid id)
    {
        var veiculo = await _veiculoService.GetByIdAsync(id);
        if (veiculo == null)
            return NotFound(new { message = "Veículo não encontrado." });
        return Ok(veiculo);
    }

    /// <summary>
    /// Busca um veículo por placa
    /// </summary>
    /// <param name="placa">Placa do veículo (normalizada automaticamente)</param>
    /// <returns>Dados do veículo</returns>
    /// <remarks>
    /// A placa é normalizada automaticamente:
    /// - Convertida para maiúsculas
    /// - Espaços e hífens removidos
    /// 
    /// Exemplos equivalentes: "ABC-1234", "abc1234", "ABC 1234"
    /// </remarks>
    /// <response code="200">Veículo encontrado</response>
    /// <response code="404">Veículo não encontrado</response>
    [HttpGet("placa/{placa}")]
    [ProducesResponseType(typeof(VeiculoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VeiculoDto>> GetByPlaca([FromRoute] string placa)
    {
        var veiculo = await _veiculoService.GetByPlacaAsync(placa);
        if (veiculo == null)
            return NotFound(new { message = "Veículo não encontrado." });
        return Ok(veiculo);
    }
}

