namespace GoodExpense.Invoicing.Domain;

public interface IInvoiceGenerator
{
    Task<string> GenerateExpenseReportAsync(CreateInvoiceRequest request);
}