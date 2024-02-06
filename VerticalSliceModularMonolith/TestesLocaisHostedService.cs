namespace VerticalSliceModularMonolith;

public class TestesLocaisHostedService : BackgroundService
{
    private readonly IWebHostEnvironment _env;
    private readonly IServiceProvider _serviceProvider;

    public TestesLocaisHostedService(
        IWebHostEnvironment env,
        IServiceProvider serviceProvider)
    {
        _env = env;
        _serviceProvider = serviceProvider;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (_env.EnvironmentName == "Development")
            {
                //var scope = _serviceProvider.CreateScope();
                //var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            }
        }
        catch (Exception e)
        {
        }
    }
}
