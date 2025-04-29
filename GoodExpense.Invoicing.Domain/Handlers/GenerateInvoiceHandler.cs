using GoodExpense.Common.Domain.Bus;
using GoodExpense.Invoicing.Domain.Events;

namespace GoodExpense.Invoicing.Domain.Handlers;

public class GenerateInvoiceHandler : IEventHandler<GenerateInvoiceEvent>
{
    private readonly IEventBus _eventBus;
    private readonly IInvoiceGenerator _invoiceGenerator;

    public GenerateInvoiceHandler(IEventBus eventBus, IInvoiceGenerator invoiceGenerator)
    {
        _eventBus = eventBus;
        _invoiceGenerator = invoiceGenerator;
    }

    public async Task Handle(GenerateInvoiceEvent eventArgs)
    {
        var generateInvoiceResult = await _invoiceGenerator.GenerateExpenseReportAsync(eventArgs.CreateInvoiceRequest);
        if (string.IsNullOrEmpty(generateInvoiceResult))
        {
            throw new InvalidOperationException("Failed to generate invoice.");
        }
        await _eventBus.Publish(new NotifyEvent
        {
            Subject = "Good Expense - Invoice Generated",
            Body = "Your invoice has been generated.",
            Recipient = eventArgs.CreateInvoiceRequest.AuthorEmail,
            Attachments = 
            [
                new AddAttachmentRequest
                {
                    FileName = $"Invoice_{eventArgs.CreateInvoiceRequest.AuthorUserName}_{eventArgs.Timestamp}.pdf",
                    ContentType = "application/pdf",
                    ContentBase64 = generateInvoiceResult,
                },
            ],
        });
    }
}