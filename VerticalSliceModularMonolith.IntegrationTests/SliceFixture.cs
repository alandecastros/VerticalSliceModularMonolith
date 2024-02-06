using Dapper;
using DotNet.Testcontainers.Containers;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace VerticalSliceModularMonolith.IntegrationTests;

[CollectionDefinition(nameof(SliceFixture))]
public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }

public class SliceFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer container;
    private RequestExecutorProxy executor;
    private IServiceScopeFactory? scopeFactory;

    public string ConnectionString
    {
        get
        {
            return container.GetConnectionString();
        }
    }

    public SliceFixture()
    {
        Environment.SetEnvironmentVariable("DOCKER_HOST", "tcp://localhost:2376");
        //Environment.SetEnvironmentVariable("TESTCONTAINERS_RYUK_CONTAINER_IMAGE", "nexus.petrobras.com.br:5000/testcontainers/ryuk:latest");

        container = new PostgreSqlBuilder()
            .WithDatabase("postgres")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithImage("postgres:14")
            .Build();
    }

    public async Task InitializeAsync()
    {
        try
        {
            await container.StartAsync();

            var script = File.ReadAllText("./Scripts/schema.sql");

            using (IDbConnection db = new NpgsqlConnection(ConnectionString))
            {
                await db.ExecuteAsync(script);
            }

            var factory = new VerticalSliceModularMonolithAppFactory(ConnectionString);

            executor = new RequestExecutorProxy(factory.Services.GetRequiredService<IRequestExecutorResolver>(), Schema.DefaultName);

            scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> ExecuteRequestAsync(
        Action<IQueryRequestBuilder> configureRequest,
        CancellationToken cancellationToken = default)
    {
        await using var scope = scopeFactory!.CreateAsyncScope();

        var requestBuilder = new QueryRequestBuilder();
        requestBuilder.SetServices(scope.ServiceProvider);
        configureRequest(requestBuilder);
        var request = requestBuilder.Create();

        await using var result = await executor.ExecuteAsync(request, cancellationToken);

        result.ExpectQueryResult();

        return result.ToJson();
    }

    public Task<ExecResult> ExecScriptAsync(string script)
    {
        return container.ExecScriptAsync(script);
    }

    public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
    {
        using var scope = scopeFactory!.CreateScope();

        try
        {
            await action(scope.ServiceProvider);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
    {
        using var scope = scopeFactory!.CreateScope();

        try
        {
            var result = await action(scope.ServiceProvider);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DisposeAsync()
    {
        await container.DisposeAsync();
    }
}

