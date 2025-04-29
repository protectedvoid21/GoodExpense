using GoodExpense.Common.Domain.Commands;
using GoodExpense.Common.Domain.Events;

namespace GoodExpense.Common.Domain.Bus;

public interface IEventBus
{
    Task SendCommand<T>(T command) where T : Command;

    Task Publish<T>(T publishEvent) where T : Event;

    Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>;
    
    Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : Request;
}