namespace FinanceControl.Application.Interfaces;

/// <summary>
/// Serviço para hash e verificação de senhas
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Gera hash de uma senha
    /// </summary>
    string Hash(string password);
    /// <summary>
    /// Verifica se a senha corresponde ao hash
    /// </summary>
    bool Verify(string password, string passwordHash);
}