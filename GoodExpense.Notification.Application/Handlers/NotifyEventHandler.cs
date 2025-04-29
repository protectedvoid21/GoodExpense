using GoodExpense.Common.Domain.Bus;
using GoodExpense.Notification.Domain;
using GoodExpense.Notification.Domain.Events;

namespace GoodExpense.Notification.Application.Handlers;

public class NotifyEventHandler : IEventHandler<NotifyEvent>
{
    private readonly INotificationService _notificationService;

    public NotifyEventHandler(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Handle(NotifyEvent request)
    {
        await _notificationService.SendNotificationAsync(new NotifyRequest
        {
            Recipients = request.Recipients,
            Subject = request.Subject,
            Body = request.Body,
            Attachments = request.Attachments
        });
    }
}