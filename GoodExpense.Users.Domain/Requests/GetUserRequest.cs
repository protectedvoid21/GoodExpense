using GoodExpense.Common.Domain;

namespace GoodExpense.Users.Domain.Requests;

public record GetUserRequest : Request
{
    public int Id { get; }

    public GetUserRequest(int id)
    {
        Id = id;
    }
}