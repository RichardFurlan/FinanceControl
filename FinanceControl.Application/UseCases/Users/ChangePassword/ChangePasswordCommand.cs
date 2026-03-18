using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.ChangePassword;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword,
    string ConfirmPassword
) : IRequest<ResultViewModel>;