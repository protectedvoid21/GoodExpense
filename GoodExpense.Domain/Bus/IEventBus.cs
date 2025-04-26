using GoodExpense.Domain.Commands;
using GoodExpense.Domain.Events;

namespace GoodExpense.Domain.Bus;

public interface IEventBus
{
    Task SendCommand<T>(T command) where T : Command;

    Task Publish<T>(T publishEvent) where T : Event;

    Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
}