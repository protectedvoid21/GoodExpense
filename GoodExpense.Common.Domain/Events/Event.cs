namespace GoodExpense.Common.Domain.Events;

public abstract record Event
{
    public DateTime Timestamp => DateTime.Now;
}