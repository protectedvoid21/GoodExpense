namespace GoodExpense.Common.Domain;

public record Request
{
    public Guid RequestId { get; } = Guid.NewGuid();
}