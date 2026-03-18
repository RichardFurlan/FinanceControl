using FluentValidation;

namespace FinanceControl.Application.UseCases.Accounts.UpdateAccount;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Account ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Account name is required")
            .MaximumLength(100)
            .WithMessage("Account name must not exceed 100 characters");
    }
}