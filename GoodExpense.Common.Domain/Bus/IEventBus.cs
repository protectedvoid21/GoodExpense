using GoodExpense.Common.Domain.Commands;
using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Common.Domain.Bus;

public interface IEventBus
{
    Task Publish<T>(T publishEvent) where T : Event;

    Task Subscribe<TEvent, TEventHandler>() where TEvent : Event where TEventHandler : IEventHandler<TEvent>;
    
    Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : Request;
}