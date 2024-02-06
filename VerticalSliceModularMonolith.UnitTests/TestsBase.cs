using Bogus;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Shared.Abstractions;
using NSubstitute.Extensions;

namespace VerticalSliceModularMonolith.UnitTests;

public class TestsBase : IAsyncLifetime
{
    protected readonly Faker faker;

    public TestsBase()
    {
        faker = new Faker("pt_BR");
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected AppDbContext CreateMockedDbContext()
    {
        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .Options;

        var eventBusMock = Substitute.For<IEventBus>();

        var dbContext = Substitute.For<AppDbContext>(dbContextOptions, eventBusMock);

        dbContext.Configure()
            .SaveChangesAsync()
            .Returns(Task.FromResult(0));

        var dbContextTransaction = Substitute.For<IDbContextTransaction>();

        var databaseFacade = Substitute.ForPartsOf<DatabaseFacade>(dbContext);

        databaseFacade.Configure()
            .BeginTransaction()
            .Returns(dbContextTransaction);

        dbContext
            .Configure()
            .Database
            .Returns(databaseFacade);

        return dbContext;
    }
}

