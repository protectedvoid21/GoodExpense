using GoodExpense.Common.Domain.Events;
using MediatR;

namespace GoodExpense.Users.Domain.Events;

public record NotifyEvent : Event
{
    public required IEnumerable<string> Recipients { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}