using System.Text;
using System.Text.Json;
using GoodExpense.Domain.Bus;
using GoodExpense.Domain.Commands;
using GoodExpense.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GoodExpense.Application;

public sealed class RabbitMQBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes = [];
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
    {
        _mediator = mediator;
        _handlers = new Dictionary<string, List<Type>>();
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public async Task Publish<T>(T publishEvent) where T : Event
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using (var connection = await factory.CreateConnectionAsync())
        using (var channel = await connection.CreateChannelAsync())
        {
            var eventName = publishEvent.GetType().Name;

            await channel.QueueDeclareAsync(eventName, false, false, false, null);

            var message = JsonSerializer.Serialize(publishEvent);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync("", eventName, body);
        }
    }

    public async Task Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName, []);
        }

        if (_handlers[eventName].Any(s => s.GetType() == handlerType))
        {
            throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for {eventName}",
                nameof(handlerType));
        }

        _handlers[eventName].Add(handlerType);

        await StartBasicConsumeAsync<T>();
    }

    private async Task StartBasicConsumeAsync<T>() where T : Event
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        var eventName = typeof(T).Name;
        await channel.QueueDeclareAsync(eventName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += Consumer_Received;

        await channel.BasicConsumeAsync(eventName, true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
    {
        var eventName = @event.RoutingKey;
        var message = Encoding.UTF8.GetString(@event.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (!_handlers.TryGetValue(eventName, out var subscriptions))
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        
        foreach (var subscription in subscriptions)
        {
            var handler = scope.ServiceProvider.GetService(subscription);

            if (handler == null)
            {
                continue;
            }

            var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
            var @event = JsonSerializer.Deserialize(message, eventType);

            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
        }
    }
}