using SistemaControleEstacionamento.Application.DTOs.Movimentacao;

namespace SistemaControleEstacionamento.Application.Interfaces;

public interface IMovimentacaoService
{
    Task<SessaoDto> RegistrarEntradaAsync(RegistrarEntradaDto dto);
    Task<SessaoDto> RegistrarSaidaAsync(RegistrarSaidaDto dto);
    Task<decimal> CalcularValorAsync(Guid sessaoId);
    Task<IEnumerable<SessaoDto>> GetSessoesAtivasAsync(string? placa = null);
}

