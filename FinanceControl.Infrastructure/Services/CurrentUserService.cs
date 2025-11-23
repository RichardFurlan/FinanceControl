using System.Security.Claims;
using FinanceControl.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinanceControl.Infrastructure.Services;

/// <summary>
/// Obtém informações do usuário logado via HttpContext
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId 
    {  
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                ?.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        } 
    }

    public string? UserEmail
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User
                ?.FindFirstValue(ClaimTypes.Email);
        }
    }

    public bool IsAuthenticated
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}