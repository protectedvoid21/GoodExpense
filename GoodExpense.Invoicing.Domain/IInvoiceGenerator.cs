namespace GoodExpense.Invoicing.Domain;

public interface IInvoiceGenerator
{
    Task<string?> GenerateInvoiceAsync(CreateInvoiceRequest request);
    
    Task<string?> GenerateExpenseRangeReportAsync(CreateRangeInvoiceRequest request);
}