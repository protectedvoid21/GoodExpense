using GoodExpense.Common.Domain;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Common.Domain.Dto;
using GoodExpense.Expenses.Domain.Commands;
using GoodExpense.Expenses.Domain.Events;
using GoodExpense.Expenses.Domain.Models;
using MediatR;
using ExpenseParticipant = GoodExpense.Expenses.Domain.Commands.ExpenseParticipant;

namespace GoodExpense.Expenses.Application.Handlers;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, bool>
{
    private readonly ExpenseDbContext _dbContext;
    private readonly IGoodExpenseClient _client;
    private readonly IEventBus _eventBus;

    public CreateExpenseCommandHandler(
        ExpenseDbContext dbContext,
        IGoodExpenseClient client,
        IEventBus eventBus)
    {
        _dbContext = dbContext;
        _client = client;
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var authorUser = await _client.GetUserAsync(request.AuthorId);
        if (authorUser.Data == null)
        {
            throw new ArgumentException($"Author user with ID {request.AuthorId} not found.");
        }
        var usersInExpense = await GetUsersInExpenseAsync(request.Participants);
        
        decimal sumAmount = request.Participants.Sum(p => p.Amount);
        
        var expense = new Expense
        {
            Title = request.Title,
            Description = request.Description,
            AuthorId = request.AuthorId,
            Amount = sumAmount,
        };
        _dbContext.Add(expense);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        foreach (var participant in request.Participants)
        {
            var expenseParticipant = new ExpenseUser
            {
                UserId = participant.UserId,
                Amount = participant.Amount,
                ExpenseId = expense.Id,
            };
            _dbContext.Add(expenseParticipant);
        }
        await _dbContext.SaveChangesAsync(cancellationToken);

        await NotifyParticipantsAsync(expense, usersInExpense, request.Participants);
        
        return true;
    }
    
    private async Task<List<GetUserDto>> GetUsersInExpenseAsync(IEnumerable<ExpenseParticipant> participants)
    {
        var users = new List<GetUserDto>();
        foreach (var participant in participants)
        {
            var user = await _client.GetUserAsync(participant.UserId);
            if (user.Data == null)
            {
                throw new ArgumentException($"User with ID {participant.UserId} not found.");
            }
            users.Add(user.Data);
        }
        return users;
    }

    private async Task NotifyParticipantsAsync(Expense expense, IEnumerable<GetUserDto> users, IEnumerable<ExpenseParticipant> expenseUsers)
    { 
        var userParticipants = users.Join(expenseUsers, 
            user => user.Id,
            participant => participant.UserId, (getUserDto, expParticipant)  => new
            {
                UserId = getUserDto.Id,
                UserName = getUserDto.UserName,
                Email = getUserDto.Email,
                Amount = expParticipant.Amount,
            }).ToList();
        
        await _eventBus.Publish(new NotifyEvent
        {
            Body = $"You have been added to an expense: {expense.Title}",
            Recipients = userParticipants.Select(u => u.Email),
            Subject = "You have been added to an expense",
        });

        await _eventBus.Publish(new CreateExpenseEvent
        {
            Expense = new ExpenseInfo
            {
                Title = expense.Title,
                Description = expense.Description,
                CreatedDate = expense.CreatedAt,
                ParticipantUsers = userParticipants.Select(u => new Domain.Events.ExpenseParticipant
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    Amount = u.Amount,
                }),
            },
        });
    }
}