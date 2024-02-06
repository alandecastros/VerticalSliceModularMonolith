using Hangfire;

namespace VerticalSliceModularMonolith.Infrastructure;

public static class ConfigureApplication
{
    public static void UseInfrastructure(this WebApplication app)
    {
        app.UseExceptionHandler();
        app.UseHangfireDashboard();
    }
}
