namespace GoodExpense.Notification.Domain;

public record AddAttachmentRequest
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required string ContentBase64 { get; set; }
}