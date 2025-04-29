namespace GoodExpense.Notification.Domain;

public interface INotificationService
{
    Task SendNotificationAsync(NotifyRequest request);
}