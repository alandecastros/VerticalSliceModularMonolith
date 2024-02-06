using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Shared.Models;
using Xunit;

namespace VerticalSliceModularMonolith.IntegrationTests.GraphQL.Queries.UsuariosOffset;

[Collection(nameof(SliceFixture))]
public class UsuariosOffsetQueryTests : TestsBase
{
    public UsuariosOffsetQueryTests(SliceFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Ok()
    {
        // Arrange
        await fixture.ExecuteScopeAsync(async sp =>
        {
            var dbContext = sp.GetRequiredService<AppDbContext>();

            var usuario = new UsuarioModel
            {
                Nome = faker.Person.FullName
            };

            await dbContext.Set<UsuarioModel>().AddAsync(usuario);

            await dbContext.SaveChangesAsync();
        });

        // Act
        var json = await fixture.ExecuteRequestAsync(
            b => b.SetQueryId("USUARIOS_DATA_TABLE")
                .SetVariableValue("skip", 0)
                .SetVariableValue("take", 10)
        );

        // Assert
        json.Should()
            .NotContain("errors");
    }
}
