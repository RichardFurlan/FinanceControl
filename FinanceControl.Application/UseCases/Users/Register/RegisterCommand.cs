using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.Register;

public record RegisterCommand(
    string Name,
    string Email,
    string Password,
    string ConfirmPassword
) : IRequest<ResultViewModel<UserDto>>;