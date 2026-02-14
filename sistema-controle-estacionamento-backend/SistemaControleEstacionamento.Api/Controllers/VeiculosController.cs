using Microsoft.AspNetCore.Mvc;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;
using SistemaControleEstacionamento.Application.Interfaces;

namespace SistemaControleEstacionamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly IVeiculoService _veiculoService;

    public VeiculosController(IVeiculoService veiculoService)
    {
        _veiculoService = veiculoService;
    }

    [HttpPost]
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

    [HttpPut("{id}")]
    public async Task<ActionResult<VeiculoDto>> Update(Guid id, [FromBody] UpdateVeiculoDto dto)
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VeiculoDto>>> GetAll()
    {
        var veiculos = await _veiculoService.GetAllAsync();
        return Ok(veiculos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VeiculoDto>> GetById(Guid id)
    {
        var veiculo = await _veiculoService.GetByIdAsync(id);
        if (veiculo == null)
            return NotFound(new { message = "Veículo não encontrado." });
        return Ok(veiculo);
    }

    [HttpGet("placa/{placa}")]
    public async Task<ActionResult<VeiculoDto>> GetByPlaca(string placa)
    {
        var veiculo = await _veiculoService.GetByPlacaAsync(placa);
        if (veiculo == null)
            return NotFound(new { message = "Veículo não encontrado." });
        return Ok(veiculo);
    }
}

