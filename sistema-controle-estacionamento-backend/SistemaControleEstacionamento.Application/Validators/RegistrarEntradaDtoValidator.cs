using FluentValidation;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;

namespace SistemaControleEstacionamento.Application.Validators;

public class RegistrarEntradaDtoValidator : AbstractValidator<RegistrarEntradaDto>
{
    public RegistrarEntradaDtoValidator()
    {
        RuleFor(x => x.VeiculoId)
            .NotEmpty().WithMessage("O ID do veículo é obrigatório");
    }
}

