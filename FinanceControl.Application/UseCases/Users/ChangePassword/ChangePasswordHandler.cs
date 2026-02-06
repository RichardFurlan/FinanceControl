using FinanceControl.Application.DTOs;
using FinanceControl.Application.Interfaces;
using FinanceControl.Domain.Interfaces;
using MediatR;

namespace FinanceControl.Application.UseCases.Users.ChangePassword;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResultViewModel>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IPasswordHasher _passwordHasher;

    public ChangePasswordHandler(IUserRepository userRepository, ICurrentUserService currentUser, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResultViewModel> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;
        if (!userId.HasValue)
            return ResultViewModel.Unauthorized("User not authenticated");

        var user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);
        if (user == null)
            return ResultViewModel.NotFound("User not found");
        
        if (!_passwordHasher.Verify(request.CurrentPassword, user.PasswordHash))
            return ResultViewModel.Error("Current password is incorrect");
        
        var newPasswordHash = _passwordHasher.Hash(request.NewPassword);
        user.UpdatePassword(newPasswordHash);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return ResultViewModel.Success("Password changed successfully");
    }
}