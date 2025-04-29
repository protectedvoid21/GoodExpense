using GoodExpense.Common.Domain.Bus;
using GoodExpense.Invoicing.Domain.Events;

namespace GoodExpense.Invoicing.Domain.Handlers;

public class GenerateRangeInvoiceHandler : IEventHandler<GenerateRangeInvoiceEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IInvoiceGenerator _invoiceGenerator;

    public GenerateRangeInvoiceHandler(IEventBus eventBus, IInvoiceGenerator invoiceGenerator)
    {
        _eventBus = eventBus;
        _invoiceGenerator = invoiceGenerator;
    }

    public async Task Handle(GenerateRangeInvoiceEvent eventArgs)
    {
        var generateInvoiceResult = await _invoiceGenerator.GenerateExpenseRangeReportAsync(eventArgs.CreateRangeInvoiceRequest);
        if (string.IsNullOrEmpty(generateInvoiceResult))
        {
            throw new InvalidOperationException("Failed to generate invoice.");
        }
        await _eventBus.Publish(new NotifyEvent
        {
            Subject = "Good Expense - Invoice Generated",
            Body = "Your invoice has been generated.",
            Recipients = [eventArgs.CreateRangeInvoiceRequest.AuthorEmail],
            Attachments = 
            [
                new AddAttachmentRequest
                {
                    FileName = $"Invoice_{eventArgs.CreateRangeInvoiceRequest.AuthorUserName}_{eventArgs.Timestamp}.pdf",
                    ContentType = "application/pdf",
                    ContentBase64 = generateInvoiceResult,
                },
            ],
        });
    }
}