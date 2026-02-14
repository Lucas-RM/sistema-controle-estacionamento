using FluentValidation;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;

namespace SistemaControleEstacionamento.Application.Validators;

public class UpdateVeiculoDtoValidator : AbstractValidator<UpdateVeiculoDto>
{
    public UpdateVeiculoDtoValidator()
    {
        RuleFor(x => x.Tipo)
            .IsInEnum()
            .When(x => x.Tipo.HasValue)
            .WithMessage("Tipo de veículo inválido");
    }
}

