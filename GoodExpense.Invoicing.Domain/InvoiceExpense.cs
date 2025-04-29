namespace GoodExpense.Invoicing.Domain;


public record CreateInvoiceRequest
{
    public required DateTime FromDate { get; set; }
    public required DateTime ToDate { get; set; }
    public required DateTime RequestedAt { get; set; }
    
    public required string AuthorUserName { get; set; }
    public required string AuthorEmail { get; set; }
    public required IEnumerable<InvoiceExpense> Expenses { get; set; }
}

public record InvoiceExpense
{
    public required string Title { get; set; } 
    public required string Description { get; set; }
    public required DateTime CreatedDate { get; set; }
    public required IEnumerable<InvoiceExpenseParticipant> ParticipantUsers { get; set; }
}

public record InvoiceExpenseParticipant
{
    public required string UserName { get; set; }
    public required decimal Amount { get; set; }
}