using System.Text;
using System.Text.Json;
using GoodExpense.Common.Domain;
using GoodExpense.Common.Domain.Bus;
using GoodExpense.Common.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GoodExpense.Common.Application;

public sealed class RabbitMqBus : IEventBus
{
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes = [];
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<RabbitMqBus> _logger;

    private readonly Uri _brokerUri;

    public RabbitMqBus(IServiceScopeFactory serviceScopeFactory, ILogger<RabbitMqBus> logger, IOptions<EventBusConfiguration> eventBusOptions)
    {
        _handlers = new Dictionary<string, List<Type>>();
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _brokerUri = new Uri(eventBusOptions.Value.BrokerUri);
    }

    public async Task Publish<TEvent>(TEvent publishEvent) where TEvent : Event
    {
        _logger.LogInformation("Publishing event {EventName}", publishEvent.GetType().Name);
        var factory = new ConnectionFactory { Uri = _brokerUri };
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

    public async Task Subscribe<TEvent, THandler>() where TEvent : Event where THandler : IEventHandler<TEvent>
    {
        var eventName = typeof(TEvent).Name;
        var handlerType = typeof(THandler);

        if (!_eventTypes.Contains(typeof(TEvent)))
        {
            _eventTypes.Add(typeof(TEvent));
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

        await StartBasicConsumeAsync<TEvent>();
    }

    public async Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : Request
    {
        var factory = new ConnectionFactory { Uri = _brokerUri };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        
        var requestName = request.GetType().Name;
        var responseName = typeof(TResponse).Name;

        await channel.QueueDeclareAsync(requestName, false, false, false);
        await channel.QueueDeclareAsync(responseName, false, false, false);

        var jsonMessage = JsonSerializer.Serialize(request);
        var encodedByteMessage = Encoding.UTF8.GetBytes(jsonMessage);

        await channel.BasicPublishAsync("", requestName, encodedByteMessage);

        var consumer = new AsyncEventingBasicConsumer(channel);
        var tcs = new TaskCompletionSource<TResponse>();

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var responseMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
            var response = JsonSerializer.Deserialize<TResponse>(responseMessage);
            tcs.SetResult(response);
        };

        await channel.BasicConsumeAsync(responseName, true, consumer);
        
        var response = await tcs.Task;
        return response;
    }

    private async Task StartBasicConsumeAsync<T>() where T : Event
    {
        var factory = new ConnectionFactory { Uri = _brokerUri };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        var eventName = typeof(T).Name;
        await channel.QueueDeclareAsync(eventName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += Consumer_Received;

        await channel.BasicConsumeAsync(eventName, true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        _logger.LogInformation("Received event {EventName}", eventName);
        
        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing event {EventName}", eventName);
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
                _logger.LogCritical("Handler for event: {EventName} was not found!", eventName);
                continue;
            }

            var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
            var @event = JsonSerializer.Deserialize(message, eventType);

            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, [@event]);
        }
    }
}