namespace GoodExpense.Notification.Domain;

public record NotifyRequest
{
    public required string Recipient { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}