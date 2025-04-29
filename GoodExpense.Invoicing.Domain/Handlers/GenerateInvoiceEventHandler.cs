using GoodExpense.Common.Domain.Bus;
using GoodExpense.Invoicing.Domain.Events;

namespace GoodExpense.Invoicing.Domain.Handlers;

public class GenerateInvoiceEventHandler : IEventHandler<CreateExpenseEvent>
{
    private readonly IInvoiceGenerator _invoiceGenerator;
    private readonly IEventBus _eventBus;

    public GenerateInvoiceEventHandler(IInvoiceGenerator invoiceGenerator, IEventBus eventBus)
    {
        _invoiceGenerator = invoiceGenerator;
        _eventBus = eventBus;
    }

    public async Task Handle(CreateExpenseEvent eventArgs)
    {
        string? invoiceBase64 = await _invoiceGenerator.GenerateInvoiceAsync(eventArgs.Expense);
        if (invoiceBase64 == null)
        {
            throw new InvalidOperationException("Failed to generate invoice.");
        }

        await _eventBus.Publish(new NotifyEvent
        {
            Body =
                "You have been added to a new expense. Please check your GoodExpense account for details.\n In attachment you can find the invoice.",
            Subject = "Good Expense - Invoice",
            Recipients = eventArgs.Expense.ParticipantUsers.Select(p => p.Email),
            Attachments = 
            [
                new AddAttachmentRequest
                {
                    FileName = $"Invoice_{eventArgs.Expense.Title}_{eventArgs.Timestamp}.pdf",
                    ContentType = "application/pdf",
                    ContentBase64 = invoiceBase64,
                },
            ],
        });
    }
}