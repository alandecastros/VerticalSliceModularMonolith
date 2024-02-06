using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Enums;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Infrastructure.Exceptions;
using VerticalSliceModularMonolith.Infrastructure.FluentValidation;
using VerticalSliceModularMonolith.Infrastructure.Hangfire;

namespace VerticalSliceModularMonolith.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.ValidationStrategy = ValidationStrategy.All;
           
configuration.OverrideDefaultResultFactoryWith<AutoFluentValidationAutoValidationCustomResultFactory>();
        });

        var dbConnectionStringReadWrite = configuration["ConnectionStrings:PostgresReadWrite"];

        services.AddDbContext<AppDbContext>((IServiceProvider serviceProvider, DbContextOptionsBuilder options) =>
        {
            options.UseNpgsql(dbConnectionStringReadWrite)
                .EnableSensitiveDataLogging();
        });

        var dbConnectionStringReadOnly = configuration["ConnectionStrings:PostgresReadOnly"];

        services.AddDbContextPool<AppReadOnlyDbContext>((IServiceProvider serviceProvider, DbContextOptionsBuilder options) =>
        {
            options.UseNpgsql(dbConnectionStringReadOnly)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        });

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(c =>
                c.UseNpgsqlConnection(dbConnectionStringReadWrite), new PostgreSqlStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(5)
                }));

        services.AddHangfireServer(action =>
        {
            action.ServerName = $"{Environment.MachineName}:{HangfireQueues.Default}";
            action.Queues = new[] { HangfireQueues.Default };
            action.WorkerCount = 1;
        });
    }
}
