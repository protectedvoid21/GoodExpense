namespace GoodExpense.Invoicing.Domain;

public record CreateInvoiceRequest
{
    public required string Title { get; set; } 
    public required string Description { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required IEnumerable<InvoiceExpenseParticipant> ParticipantUsers { get; set; }
}

public record InvoiceExpenseParticipant
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required decimal Amount { get; set; }
}