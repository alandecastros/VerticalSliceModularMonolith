namespace VerticalSliceModularMonolith.Shared.Abstractions;

public interface ICommandDispatcher
{
    Task<object?> Send(object request, bool flushAndDispatchEvents = true, CancellationToken cancellationToken = default);
}
