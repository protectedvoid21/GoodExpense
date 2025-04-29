namespace GoodExpense.Common.Domain;

public class BaseTrackingEntity : Entity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}