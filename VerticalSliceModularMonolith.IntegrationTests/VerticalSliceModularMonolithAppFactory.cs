using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using VerticalSliceModularMonolith.IntegrationTests.Fakes;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.IntegrationTests;

public class VerticalSliceModularMonolithAppFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public VerticalSliceModularMonolithAppFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ConnectionStrings:PostgresReadWrite", _connectionString);
        Environment.SetEnvironmentVariable("ConnectionStrings:PostgresReadOnly", _connectionString);

        builder.UseEnvironment("Testing");

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IUsuarioLogadoService));
            services.AddTransient<IUsuarioLogadoService, FakeUsuarioLogadoService>();

            services.RemoveAll(typeof(IHostedService));
        });
    }
}
