using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Common.Domain.Bus;

public interface IEventHandler<in TEvent> : IEventHandler where TEvent : Event
{
    Task Handle(TEvent @event);

}

public interface IEventHandler { }