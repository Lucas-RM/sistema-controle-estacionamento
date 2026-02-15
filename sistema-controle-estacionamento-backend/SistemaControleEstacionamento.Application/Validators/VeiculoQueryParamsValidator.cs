using FluentValidation;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;

namespace SistemaControleEstacionamento.Application.Validators;

public class VeiculoQueryParamsValidator : AbstractValidator<VeiculoQueryParams>
{
    public VeiculoQueryParamsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page deve ser maior ou igual a 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize deve ser maior ou igual a 1")
            .LessThanOrEqualTo(100).WithMessage("PageSize não pode exceder 100");

        RuleFor(x => x.SortOrder)
            .Must(order => new[] { "asc", "desc" }.Contains(order.ToLower()))
            .WithMessage("SortOrder deve ser 'asc' ou 'desc'");

        RuleFor(x => x.Tipo)
            .Must(tipo => new[] { "Carro", "Moto", "Caminhonete" }.Contains(tipo))
            .When(x => !string.IsNullOrWhiteSpace(x.Tipo))
            .WithMessage("Tipo deve ser 'Carro', 'Moto' ou 'Caminhonete'");
    }
}
