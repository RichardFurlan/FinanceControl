using FinanceControl.Application.Common;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.Login;

public class LoginHandler : IRequestHandler<LoginCommand, ResultViewModel<LoginResponseDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly NotificationContext _notifications;

    public LoginHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, NotificationContext notifications)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _notifications = notifications;
    }

    public async Task<ResultViewModel<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            _notifications.AddNotification("Credentials", "Invalid email or password");
        }

        if (!user!.IsActive)
        {
            _notifications.AddNotification("User", "User account is inactive");
        }
        
        if (_notifications.HasNotifications)
            return ResultViewModel<LoginResponseDto>.ValidationError(_notifications.GetValidationErrors());
        
        user.RegisterLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);
        
        var token = _tokenService.GenerateToken(user);

        var response = new LoginResponseDto
        {
            User = UserDto.FromEntity(user),
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        };

        return ResultViewModel<LoginResponseDto>.Success(response, "Login successful");
    }
}