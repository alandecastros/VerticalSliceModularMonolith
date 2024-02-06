using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using VerticalSliceModularMonolith.Infrastructure.Database;
using Xunit;

namespace VerticalSliceModularMonolith.IntegrationTests;

public class TestsBase : IAsyncLifetime
{
    protected readonly SliceFixture fixture;
    protected readonly Faker faker;

    public TestsBase(SliceFixture fixture)
    {
        faker = new Faker("pt_BR");
        this.fixture = fixture;
    }

    public async Task DisposeAsync()
    {
        await fixture.ExecuteScopeAsync(async sp =>
        {
            var context = sp.GetRequiredService<AppDbContext>();

            var sqlFileName = $"./Scripts/truncate.sql";
            var sqlCommands = File.ReadLines(sqlFileName);

            foreach (var sqlCommand in sqlCommands)
            {
                if (sqlCommand.Trim().Length > 0)
                {
                    await context.Database.ExecuteSqlRawAsync(sqlCommand);
                }
            }
        });
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}

