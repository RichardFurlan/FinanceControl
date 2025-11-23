using FluentValidation;

namespace FinanceControl.Application.UseCases.Accounts.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Account name is required")
            .MaximumLength(100).WithMessage("Account name cannot exceed 100 characters");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Account type is required")
            .Must(BeValidAccountType).WithMessage("Invalid account type. Valid types: CheckingAccount, SavingsAccount, Wallet, Investment");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code (e.g., BRL, USD, EUR)");

        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Initial balance cannot be negative");
    }
    
    private bool BeValidAccountType(string type)
    {
        return Enum.TryParse<Domain.Enums.AccountType>(type, out _);
    }
}