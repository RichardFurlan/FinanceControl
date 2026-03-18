using FinanceControl.Application.DTOs;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.GetUserProfile;

public record GetUserProfileQuery(Guid? UserId) : IRequest<ResultViewModel<UserDto>>;