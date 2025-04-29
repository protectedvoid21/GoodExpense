namespace GoodExpense.Common.Domain;

public abstract record Response
{
    public required Guid RequestId { get; set; }
}