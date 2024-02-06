namespace VerticalSliceModularMonolith.Shared.Abstractions;

public interface IEventBus
{
    void AddEvent(DomainEvent e);
    Task FlushDomainEventsAsync(CancellationToken cancellationToken = default);
    Task FlushIntegrationEventsAsync(CancellationToken cancellationToken = default);
}
