namespace GoodExpense.Notification.Application;

public class AzureNotificationConfiguration
{
    public required string ConnectionString { get; set; }
    public required string FromAddress { get; set; }
}