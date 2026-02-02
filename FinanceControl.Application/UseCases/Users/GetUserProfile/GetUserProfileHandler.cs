using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.GetUserProfile;

public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, ResultViewModel<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;

    public GetUserProfileHandler(IUserRepository userRepository, ICurrentUserService currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public async Task<ResultViewModel<UserDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ??_currentUser.UserId;

        if (!userId.HasValue)
        {
            return ResultViewModel<UserDto>.Unauthorized("User not authenticated");
        }
        
        var user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);
        
        if (user == null)
            return ResultViewModel<UserDto>.NotFound("User not found");
        
        return  ResultViewModel<UserDto>.Success(UserDto.FromEntity(user));
    }
}