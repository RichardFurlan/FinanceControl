using FluentValidation;

namespace FinanceControl.Application.UseCases.Accounts.Transfer;

public class TransferValidator : AbstractValidator<TransferCommand>
{
    public TransferValidator()
    {
        RuleFor(x => x.SourceAccountId)
            .NotEmpty()
            .WithMessage("Conta de origem é obrigatória");

        RuleFor(x => x.DestinationAccountId)
            .NotEmpty()
            .WithMessage("Conta de destino é obrigatória");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Valor deve ser maior que zero");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Descrição é obrigatória")
            .MaximumLength(200)
            .WithMessage("Descrição deve ter no máximo 200 caracteres");

        RuleFor(x => x)
            .Must(x => x.SourceAccountId != x.DestinationAccountId)
            .WithMessage("Conta de origem e destino não podem ser iguais");
    }
}