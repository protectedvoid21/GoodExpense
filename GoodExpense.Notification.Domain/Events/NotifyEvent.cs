using GoodExpense.Common.Domain.Events;
using MediatR;

namespace GoodExpense.Notification.Domain.Events;

public record NotifyEvent : Event
{
    public required string Recipient { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}