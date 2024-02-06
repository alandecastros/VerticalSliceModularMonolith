using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Shared.Abstractions;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.Infrastructure.Database;

public class AppDbContext : DbContext
{
    private readonly IEventBus _eventBus;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IEventBus eventBus) : base(options)
    {
        _eventBus = eventBus;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioModelConfig).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var domainEventsQueues = ChangeTracker.Entries<BaseEntity>()
            .Select(po => po.Entity.DomainEvents)
            .ToList();

        foreach (var domainEventQueue in domainEventsQueues)
        {
            while (domainEventQueue.Count > 0)
            {
                var @event = domainEventQueue.Dequeue();
                _eventBus.AddEvent(@event);
            }
        }

        var response = await base.SaveChangesAsync(cancellationToken);

        return response;
    }
}
