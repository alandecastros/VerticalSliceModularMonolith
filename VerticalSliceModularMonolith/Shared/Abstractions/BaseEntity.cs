using System.ComponentModel.DataAnnotations.Schema;

namespace VerticalSliceModularMonolith.Shared.Abstractions;

public class BaseEntity
{
    protected BaseEntity()
    {
    }

    [NotMapped]
    [GraphQLIgnore]
    public Queue<DomainEvent> DomainEvents { get; set; } = new Queue<DomainEvent>();

    public void AddDomainEvent(DomainEvent notification)
    {
        DomainEvents.Enqueue(notification);
    }
}
