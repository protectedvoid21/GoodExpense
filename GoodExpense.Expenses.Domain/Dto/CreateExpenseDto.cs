namespace GoodExpense.Expenses.Domain.Dto;

public class CreateExpenseDto
{
    public required string Description { get; set; }
    public required string Title { get; set; }
    public required int AuthorId { get; set; }
    public required IEnumerable<ParticipantDto> Participants { get; set; } = [];
}

public class ParticipantDto
{
    public required int UserId { get; set; }
    public required decimal Amount { get; set; }
}