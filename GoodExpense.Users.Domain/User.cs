using GoodExpense.Common.Domain;

namespace GoodExpense.Users.Domain;

public class User : BaseTrackingEntity
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}