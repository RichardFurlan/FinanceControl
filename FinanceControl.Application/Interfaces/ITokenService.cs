using FinanceControl.Domain.Entities;

namespace FinanceControl.Application.Interfaces;
/// <summary>
/// Serviço para geração de tokens JWT
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Gera um token JWT para o usuário
    /// </summary>
    string GenerateToken(User user);
}