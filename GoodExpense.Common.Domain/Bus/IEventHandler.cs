using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Common.Domain.Bus;

public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
{
    Task Handle(TEvent eventArgs);
}

public interface IEventHandler;