using FluentValidation;
using SistemaControleEstacionamento.Application.DTOs.Movimentacao;

namespace SistemaControleEstacionamento.Application.Validators;

public class SessaoQueryParamsValidator : AbstractValidator<SessaoQueryParams>
{
    public SessaoQueryParamsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page deve ser maior ou igual a 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize deve ser maior ou igual a 1")
            .LessThanOrEqualTo(100).WithMessage("PageSize não pode exceder 100");

        RuleFor(x => x.Status)
            .Must(status => new[] { "todas", "ativas", "fechadas" }.Contains(status.ToLower()))
            .WithMessage("Status deve ser 'todas', 'ativas' ou 'fechadas'");

        RuleFor(x => x.SortOrder)
            .Must(order => new[] { "asc", "desc" }.Contains(order.ToLower()))
            .WithMessage("SortOrder deve ser 'asc' ou 'desc'");

        RuleFor(x => x.DataInicio)
            .LessThanOrEqualTo(x => x.DataFim)
            .When(x => x.DataInicio.HasValue && x.DataFim.HasValue)
            .WithMessage("DataInicio não pode ser maior que DataFim");
    }
}
