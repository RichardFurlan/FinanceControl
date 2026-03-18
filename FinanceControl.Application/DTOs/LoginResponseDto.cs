namespace FinanceControl.Application.DTOs;

public record LoginResponseDto
{
    public UserDto User { get; init; } = null!;
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
};