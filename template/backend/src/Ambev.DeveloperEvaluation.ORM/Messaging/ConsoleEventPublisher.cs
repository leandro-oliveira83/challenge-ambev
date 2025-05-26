using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Messaging;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.ORM.Messaging;

/// <summary>
/// Basic implementation that logs published events to the console or ILogger.
/// </summary>
public class ConsoleEventPublisher : IEventPublisher
{
    private readonly ILogger<ConsoleEventPublisher> _logger;

    public ConsoleEventPublisher(ILogger<ConsoleEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        _logger.LogInformation("[EVENT PUBLISHED] {EventName} at {Time}", typeof(TEvent).Name, @event.OccurredAt);
        return Task.CompletedTask;
    }
}