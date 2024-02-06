using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.IntegrationTests.Fakes;
using VerticalSliceModularMonolith.Modules.Livros.Features.SalvarLivro;
using VerticalSliceModularMonolith.Shared.Models;
using Xunit;

namespace VerticalSliceModularMonolith.IntegrationTests.Modules.Livros.Features.SalvarLivro;

[Collection(nameof(SliceFixture))]
public class SalvarLivroCommandHandlerTests : TestsBase
{
    public SalvarLivroCommandHandlerTests(SliceFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Ok()
    {
        // Arrange
        var usuarioCodigo = faker.Random.String2(10);

        //Act
        UsuarioLogado.Codigo = usuarioCodigo;

        var _cmd = new SalvarLivroCommand
        {
            Titulo = faker.Person.FullName,
        };

        await fixture.ExecuteScopeAsync(async sp =>
        {
            var controller = sp.GetRequiredService<SalvarLivroCommandHandlerController>();
            await controller.Handle(_cmd);
        });

        // Assert
        await fixture.ExecuteScopeAsync(async sp =>
        {
            var dbContext = sp.GetRequiredService<AppDbContext>();

            var livro = await dbContext.Set<LivroModel>().FirstAsync();

            livro.Titulo
                .Should()
                .Be(_cmd.Titulo);
        });
    }
}
