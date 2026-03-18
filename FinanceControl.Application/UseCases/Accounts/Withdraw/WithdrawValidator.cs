using FluentValidation;

namespace FinanceControl.Application.UseCases.Accounts.Withdraw;

public class WithdrawValidator : AbstractValidator<WithdrawCommand>
{
    public WithdrawValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
    }
}