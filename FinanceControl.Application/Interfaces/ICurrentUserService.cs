namespace FinanceControl.Application.Interfaces;

/// <summary>
/// Serviço para obter o usuário logado atual
/// </summary>
public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? UserEmail { get; }
    bool IsAuthenticated { get; }
}