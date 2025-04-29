using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Users.Domain.Events;

public record UserRetrievedEvent : Event
{
    public required int UserId { get; init; }
    public required string UserName { get; init; }
}