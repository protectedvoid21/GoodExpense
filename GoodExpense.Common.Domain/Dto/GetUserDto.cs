namespace GoodExpense.Common.Domain.Dto;

public record GetUserDto
{
    public required int Id { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}