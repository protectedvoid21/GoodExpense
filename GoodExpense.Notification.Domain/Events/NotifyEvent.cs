using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Notification.Domain.Events;

public record NotifyEvent : Event
{
    public required IEnumerable<string> Recipients { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public IEnumerable<AddAttachmentRequest> Attachments { get; set; } = [];
}