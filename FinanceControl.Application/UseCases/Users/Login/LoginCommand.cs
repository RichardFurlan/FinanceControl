using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<ResultViewModel<LoginResponseDto>>;