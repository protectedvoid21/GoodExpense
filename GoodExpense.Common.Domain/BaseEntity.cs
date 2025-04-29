namespace GoodExpense.Common.Domain;

public class BaseEntity<TKey>
{
    public TKey Id { get; set; }
}