using FluentValidation;

namespace FinanceControl.Application.UseCases.Users.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Senha atual é obrigatória");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Nova senha é obrigatória")
            .MinimumLength(6)
            .WithMessage("Nova senha deve ter no mínimo 6 caracteres");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirmação de senha é obrigatória")
            .Equal(x => x.NewPassword)
            .WithMessage("Senhas não conferem");
    }
}