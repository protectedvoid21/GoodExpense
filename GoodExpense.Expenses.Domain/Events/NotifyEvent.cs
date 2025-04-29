using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Expenses.Domain.Events;

public record NotifyEvent : Event
{
    public required IEnumerable<string> Recipients { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public IEnumerable<AddAttachmentRequest> Attachments { get; set; } = [];
}

public record AddAttachmentRequest
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required string ContentBase64 { get; set; }
}