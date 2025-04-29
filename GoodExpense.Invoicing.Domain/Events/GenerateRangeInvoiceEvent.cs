using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Invoicing.Domain.Events;

public record GenerateRangeInvoiceEvent : Event
{
    public required CreateRangeInvoiceRequest CreateRangeInvoiceRequest { get; set; }
}