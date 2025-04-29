using MediatR;

namespace GoodExpense.Users.Domain.Commands;

public class CreateUserCommand : IRequest<bool>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public required string Email { get; set; }
}