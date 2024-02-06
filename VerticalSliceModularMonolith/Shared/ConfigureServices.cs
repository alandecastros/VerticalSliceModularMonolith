using VerticalSliceModularMonolith.Shared.Abstractions;
using VerticalSliceModularMonolith.Shared.Services;

namespace VerticalSliceModularMonolith.Shared;

public static class ConfigureServices
{
    public static void AddShared(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IEventBus, EventBus>();
        services.AddScoped<IUsuarioLogadoService, UsuarioLogadoService>();
    }
}
