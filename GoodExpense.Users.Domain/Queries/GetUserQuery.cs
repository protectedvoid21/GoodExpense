using GoodExpense.Users.Domain.Dto;
using MediatR;

namespace GoodExpense.Users.Domain.Queries;

public class GetUserQuery : IRequest<GetUserDto?>
{
    public int Id { get; }
    
    public GetUserQuery(int id)
    {
        Id = id;
    }

}