namespace GoodExpense.Invoicing.Domain;

public record CreateRangeInvoiceRequest
{
    public required DateTime FromDate { get; set; }
    public required DateTime ToDate { get; set; }
    public required DateTime RequestedAt { get; set; }
    
    public required string AuthorUserName { get; set; }
    public required string AuthorEmail { get; set; }
    public required IEnumerable<CreateInvoiceRequest> Expenses { get; set; }
}