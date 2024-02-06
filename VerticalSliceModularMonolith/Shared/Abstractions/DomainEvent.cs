using MediatR;

namespace VerticalSliceModularMonolith.Shared.Abstractions;

public abstract class DomainEvent : INotification, IRequest
{
    public string EventId { get; set; }
    public DateTime CreatedAt { get; set; }

    protected DomainEvent()
    {
        EventId = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
    }
}
