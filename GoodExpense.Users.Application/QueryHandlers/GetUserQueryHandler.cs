using GoodExpense.Common.Domain.Bus;
using GoodExpense.Users.Domain.Dto;
using GoodExpense.Users.Domain.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodExpense.Users.Application.QueryHandlers;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserDto?>
{
    private readonly UsersDbContext _dbContext;

    public GetUserQueryHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetUserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Select(u => new GetUserDto
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.Username,
            })
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
    }
}