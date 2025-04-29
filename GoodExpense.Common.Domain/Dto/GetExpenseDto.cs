namespace GoodExpense.Common.Domain.Dto;

public record GetExpenseDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required decimal Amount { get; init; }
    public required DateTime CreatedOn { get; init; }
}

