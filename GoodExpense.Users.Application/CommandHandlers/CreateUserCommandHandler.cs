using GoodExpense.Common.Domain.Bus;
using GoodExpense.Users.Domain;
using GoodExpense.Users.Domain.Commands;
using GoodExpense.Users.Domain.Events;
using GoodExpense.Users.Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Users.Application.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, (string Message, bool IsSuccess)>
{
    private readonly UsersDbContext _dbContext;
    private readonly IPasswordEncrypter _passwordEncrypter;
    private readonly IEventBus _eventBus;

    public CreateUserCommandHandler(UsersDbContext dbContext, IPasswordEncrypter passwordEncrypter, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _passwordEncrypter = passwordEncrypter;
        _eventBus = eventBus;
    }

    public async Task<(string Message, bool IsSuccess)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Username == request.UserName || u.Email == request.Email, cancellationToken))
        {
            return ("User already exists!", false);
        }
        
        var user = new User
        {
            Username = request.UserName,
            Email = request.Email,
            Password = _passwordEncrypter.Encrypt(request.Password),
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _eventBus.Publish(new NotifyEvent
        {
            Body = "Congratulations! You have been successfully registered! For more information, please visit our website.",
            Subject = "Good Expense - Registration Confirmation",
            Recipient = request.Email,
        });

        return ("User created successfully!", true);
    }
}