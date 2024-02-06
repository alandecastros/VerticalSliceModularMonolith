using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Shared.Models;
using Xunit;

namespace VerticalSliceModularMonolith.IntegrationTests.GraphQL.Queries.LivrosOffset;

[Collection(nameof(SliceFixture))]
public class LivrosOffsetQueryTests : TestsBase
{
    public LivrosOffsetQueryTests(SliceFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Ok()
    {
        // Arrange
        await fixture.ExecuteScopeAsync(async sp =>
        {
            var dbContext = sp.GetRequiredService<AppDbContext>();

            var livro = new LivroModel
            {
                Titulo = faker.Person.FullName
            };

            await dbContext.Set<LivroModel>().AddAsync(livro);

            await dbContext.SaveChangesAsync();
        });

        // Act
        var json = await fixture.ExecuteRequestAsync(
            b => b.SetQueryId("LIVROS_DATA_TABLE")
                .SetVariableValue("skip", 0)
                .SetVariableValue("take", 10)
        );

        // Assert
        json.Should()
            .NotContain("errors");
    }
}
