using MediatR;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.Shared.Services;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly AppDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IEventBus _eventBus;

    public CommandDispatcher(
        AppDbContext dbContext,
        IMediator mediator,
        IEventBus eventBus)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _eventBus = eventBus;
    }

    public async Task<object?> Send(object request, bool flushAndDispatchEvents = true, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(request, cancellationToken);

        await _eventBus.FlushDomainEventsAsync(cancellationToken);

        if (flushAndDispatchEvents)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await _eventBus.FlushIntegrationEventsAsync(cancellationToken);
        }

        return response;
    }
}
