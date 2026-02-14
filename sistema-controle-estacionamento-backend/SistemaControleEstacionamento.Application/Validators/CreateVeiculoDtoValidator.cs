using FluentValidation;
using SistemaControleEstacionamento.Application.DTOs.Veiculo;

namespace SistemaControleEstacionamento.Application.Validators;

public class CreateVeiculoDtoValidator : AbstractValidator<CreateVeiculoDto>
{
    public CreateVeiculoDtoValidator()
    {
        RuleFor(x => x.Placa)
            .NotEmpty().WithMessage("A placa é obrigatória")
            .Must(ValidarPlaca).WithMessage("Formato de placa inválido. Use formato brasileiro (AAA-9999 ou AAA9A99) ou argentino (AA999AA ou AAA999)");

        RuleFor(x => x.Tipo)
            .IsInEnum().WithMessage("Tipo de veículo inválido");
    }

    private bool ValidarPlaca(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            return false;

        // Remove espaços e converte para maiúsculo
        placa = placa.Trim().ToUpper().Replace("-", "").Replace(" ", "");

        // Formato Brasil antigo: AAA9999 (3 letras + 4 números)
        var formatoBrasilAntigo = System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}\d{4}$");

        // Formato Brasil Mercosul: AAA9A99 (3 letras + 1 número + 1 letra + 2 números)
        var formatoBrasilMercosul = System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}\d[A-Z]\d{2}$");

        // Formato Argentina novo: AA999AA (2 letras + 3 números + 2 letras)
        var formatoArgentinaNovo = System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{2}\d{3}[A-Z]{2}$");

        // Formato Argentina antigo: AAA999 (3 letras + 3 números)
        var formatoArgentinaAntigo = System.Text.RegularExpressions.Regex.IsMatch(placa, @"^[A-Z]{3}\d{3}$");

        return formatoBrasilAntigo || formatoBrasilMercosul || formatoArgentinaNovo || formatoArgentinaAntigo;
    }
}

