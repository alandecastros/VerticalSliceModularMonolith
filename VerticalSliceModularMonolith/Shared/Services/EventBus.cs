using Hangfire;
using MediatR;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.Shared.Services;

public class EventBus : IEventBus
{
    private readonly Queue<DomainEvent> _domainQueue;
    private readonly Queue<object> _integrationQueue;
    private readonly ILogger<EventBus> _logger;
    private readonly IMediator _mediator;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public EventBus(
        ILogger<EventBus> logger,
        IMediator mediator,
        IBackgroundJobClient backgroundJobClient)
    {
        _domainQueue = new Queue<DomainEvent>();
        _integrationQueue = new Queue<object>();
        _logger = logger;
        _mediator = mediator;
        _backgroundJobClient = backgroundJobClient;
    }

    public void AddEvent(DomainEvent e)
    {
        _domainQueue.Enqueue(e);
        _integrationQueue.Enqueue(e);
    }

    private DomainEvent NextDomainEvent()
    {
        return _domainQueue.Dequeue();
    }

    private bool HasDomainEvent()
    {
        return _domainQueue.Count > 0;
    }

    private object NextIntegrationEvent()
    {
        return _integrationQueue.Dequeue();
    }

    private bool HasIntegrationEvent()
    {
        return _integrationQueue.Count > 0;
    }

    public async Task FlushDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        while (HasDomainEvent())
        {
            var @event = NextDomainEvent();
            _logger.LogInformation($"Publishing {@event.GetType().Name} to dispatcher...");
            await _mediator.Publish(@event, cancellationToken);
        }
    }

    public Task FlushIntegrationEventsAsync(CancellationToken cancellationToken = default)
    {
        while (HasIntegrationEvent())
        {
            var @event = (IRequest)NextIntegrationEvent();
            _logger.LogInformation($"Publishing {@event.GetType().Name} to integration bus...");

            _backgroundJobClient.Enqueue<IMediator>(mediator => mediator.Send(@event, cancellationToken));
        }

        return Task.CompletedTask;
    }
}
