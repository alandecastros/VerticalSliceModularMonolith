using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using System.Threading;
using VerticalSliceModularMonolith.Modules.Usuarios.Abstractions;
using VerticalSliceModularMonolith.Shared.Exceptions;
using Xunit;
using VerticalSliceModularMonolith.Modules.Usuarios.Features.SalvarUsuario;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.UnitTests.Modules.Usuarios.Features.SalvarUsuario;

public class SalvarUsuarioCommandHandlerTests : TestsBase
{
    [Fact]
    public async Task NomeUsuarioJaExiste()
    {
        // Arrange
        var dbContext = CreateMockedDbContext();

        var usuarioServiceMock = Substitute.For<IUsuarioService>();
        usuarioServiceMock
            .ExisteAsync(Arg.Any<string>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var sut = new SalvarUsuarioCommandHandler(dbContext, usuarioServiceMock);
        var _cmd = new SalvarUsuarioCommand
        {
            Nome = faker.Person.FullName,
            Usuario = faker.Random.String2(10)
        };

        var act = () => sut.Handle(_cmd, default);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Usuário já existe com esse nome");
    }

    [Fact]
    public async Task Ok()
    {
        // Arrange
        var dbContext = CreateMockedDbContext();

        var usuarioServiceMock = Substitute.For<IUsuarioService>();
        usuarioServiceMock
            .ExisteAsync(Arg.Any<string>(), default)
            .Returns(Task.FromResult(false));

        // Act
        var sut = new SalvarUsuarioCommandHandler(dbContext, usuarioServiceMock);
        var _cmd = new SalvarUsuarioCommand
        {
            Nome = faker.Person.FullName,
            Usuario = faker.Random.String2(10)
        };

        await sut.Handle(_cmd, default);

        // Assert
        await dbContext.Set<UsuarioModel>().Received()
            .AddAsync(
                Arg.Is<UsuarioModel>(x => x.Nome == _cmd.Nome),
                Arg.Any<CancellationToken>()
            );
        await dbContext.Received().SaveChangesAsync();
    }
}
