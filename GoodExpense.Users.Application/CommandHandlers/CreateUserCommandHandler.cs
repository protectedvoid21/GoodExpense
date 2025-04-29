using GoodExpense.Users.Domain;
using GoodExpense.Users.Domain.Commands;
using GoodExpense.Users.Domain.Services;
using MediatR;

namespace GoodExpense.Users.Application.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly UsersDbContext _dbContext;
    private readonly IPasswordEncrypter _passwordEncrypter;

    public CreateUserCommandHandler(UsersDbContext dbContext, IPasswordEncrypter passwordEncrypter)
    {
        _dbContext = dbContext;
        _passwordEncrypter = passwordEncrypter;
    }

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Username = request.UserName,
            Email = request.Email,
            Password = _passwordEncrypter.Encrypt(request.Password)
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}