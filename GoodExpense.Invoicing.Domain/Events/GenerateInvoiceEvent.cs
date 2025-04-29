using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Invoicing.Domain.Events;

public record GenerateInvoiceEvent : Event
{
    public required CreateInvoiceRequest CreateInvoiceRequest { get; set; }
}