namespace GoodExpense.Common.Domain.Events;

public abstract record Event
{
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public DateTime Timestamp => DateTime.Now;
}