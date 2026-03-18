namespace FinanceControl.Application.Common;

/// <summary>
/// Representa uma notificação (erro de validação ou negócio)
/// </summary>
public record Notification
{
    public string Key { get; }
    public string Message { get; }

    public Notification(string key, string message)
    {
        Key = key;
        Message = message;
    }
}