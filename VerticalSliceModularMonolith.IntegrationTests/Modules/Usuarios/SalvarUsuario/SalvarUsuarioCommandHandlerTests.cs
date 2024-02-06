using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.IntegrationTests.Fakes;
using VerticalSliceModularMonolith.Modules.Usuarios.Features.SalvarUsuario;
using VerticalSliceModularMonolith.Shared.Models;
using Xunit;

namespace VerticalSliceModularMonolith.IntegrationTests.Modules.Usuarios.SalvarUsuario;

[Collection(nameof(SliceFixture))]
public class SalvarUsuarioCommandHandlerTests : TestsBase
{
    public SalvarUsuarioCommandHandlerTests(SliceFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Ok()
    {
        // Arrange
        var usuarioCodigo = faker.Random.String2(10);

        //Act
        UsuarioLogado.Codigo = usuarioCodigo;

        var _cmd = new SalvarUsuarioCommand
        {
            Nome = faker.Person.FullName,
        };

        await fixture.ExecuteScopeAsync(async sp =>
        {
            var controller = sp.GetRequiredService<SalvarUsuarioCommandHandlerController>();
            await controller.Handle(_cmd);
        });

        // Assert
        await fixture.ExecuteScopeAsync(async sp =>
        {
            var dbContext = sp.GetRequiredService<AppDbContext>();

            var usuario = await dbContext.Set<UsuarioModel>().FirstAsync();

            usuario.Nome
                .Should()
                .Be(_cmd.Nome);
        });
    }
}
