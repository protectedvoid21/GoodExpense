namespace GoodExpense.Notification.Domain;

public record NotifyRequest
{
    public required IEnumerable<string> Recipients { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public IEnumerable<AddAttachmentRequest> Attachments { get; set; } = [];
}