using FinanceControl.Application.DTOs;

namespace FinanceControl.Application.Common;

/// <summary>
/// Gerenciador de notificações (erros) sem usar exceções
/// Implementa o Notification Pattern
/// </summary>
public class NotificationContext
{
    private readonly List<Notification> _notifications = new();

    /// <summary>
    /// Lista de notificações acumuladas
    /// </summary>
    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

    /// <summary>
    /// Verifica se há notificações
    /// </summary>
    public bool HasNotifications => _notifications.Any();

    /// <summary>
    /// Verifica se está válido (sem notificações)
    /// </summary>
    public bool IsValid => !HasNotifications;

    /// <summary>
    /// Adiciona uma notificação
    /// </summary>
    public void AddNotification(string key, string message)
    {
        _notifications.Add(new Notification(key, message));
    }

    /// <summary>
    /// Adiciona uma notificação
    /// </summary>
    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
    }

    /// <summary>
    /// Adiciona múltiplas notificações
    /// </summary>
    public void AddNotifications(IEnumerable<Notification> notifications)
    {
        _notifications.AddRange(notifications);
    }

    /// <summary>
    /// Limpa todas as notificações
    /// </summary>
    public void Clear()
    {
        _notifications.Clear();
    }

    /// <summary>
    /// Converte notificações para ValidationError (usado no ResultViewModel)
    /// </summary>
    public IEnumerable<ValidationError> GetValidationErrors()
    {
        return _notifications.Select(n => new ValidationError(n.Key, n.Message));
    }

    /// <summary>
    /// Retorna todas as mensagens de erro concatenadas
    /// </summary>
    public string GetErrorMessages(string separator = "; ")
    {
        return string.Join(separator, _notifications.Select(n => n.Message));
    }
}