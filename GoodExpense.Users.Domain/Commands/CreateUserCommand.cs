using MediatR;

namespace GoodExpense.Users.Domain.Commands;

public class CreateUserCommand : IRequest<(string Message, bool IsSuccess)>
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public required string Email { get; set; }
}