using FinanceControl.Application.Common;
using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Entities;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, ResultViewModel<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly NotificationContext _notifications;

    public RegisterHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, NotificationContext notifications)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _notifications = notifications;
    }

    public async Task<ResultViewModel<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
        {
            _notifications.AddNotification(
                nameof(request.ConfirmPassword),
                "Passwords do not match"
            );
        }
        
        if (await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            _notifications.AddNotification(
                nameof(request.Email),
                "Email already registered"
            );
        }
        
        if (_notifications.HasNotifications)
            return ResultViewModel<UserDto>.ValidationError(_notifications.GetValidationErrors());
        
        var passwordHash = _passwordHasher.Hash(request.Password);
        
        var user = new User(request.Name, request.Email, passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);

        return ResultViewModel<UserDto>.Created(
            UserDto.FromEntity(user),
            "User registered successfully"
        );
    }
}