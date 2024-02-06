using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VerticalSliceModularMonolith.Modules.Livros.Abstractions;
using VerticalSliceModularMonolith.Modules.Livros.Features.SalvarLivro;
using VerticalSliceModularMonolith.Modules.Usuarios.Abstractions;
using VerticalSliceModularMonolith.Modules.Usuarios.Features.SalvarUsuario;
using VerticalSliceModularMonolith.Shared.Exceptions;
using VerticalSliceModularMonolith.Shared.Models;
using Xunit;

namespace VerticalSliceModularMonolith.UnitTests.Modules.Livros.Features.SalvarLivro;

public class SalvarLivroCommandHandlerTests : TestsBase
{
    [Fact]
    public async Task NomeUsuarioJaExiste()
    {
        // Arrange
        var dbContext = CreateMockedDbContext();

        var livroServiceMock = Substitute.For<ILivroService>();
        livroServiceMock
            .ExisteAsync(Arg.Any<string>(), default)
            .Returns(Task.FromResult(true));

        // Act
        var sut = new SalvarLivroCommandHandler(dbContext, livroServiceMock);

        var _cmd = new SalvarLivroCommand
        {
            Titulo = faker.Person.FullName,
            Usuario = faker.Random.String2(10)
        };

        var act = () => sut.Handle(_cmd, default);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("Livro já existe com esse título");
    }

    [Fact]
    public async Task Ok()
    {
        // Arrange
        var dbContext = CreateMockedDbContext();

        var livroServiceMock = Substitute.For<ILivroService>();
        livroServiceMock
            .ExisteAsync(Arg.Any<string>(), default)
            .Returns(Task.FromResult(false));

        // Act
        var sut = new SalvarLivroCommandHandler(dbContext, livroServiceMock);

        var _cmd = new SalvarLivroCommand
        {
            Titulo = faker.Person.FullName,
            Usuario = faker.Random.String2(10)
        };

        await sut.Handle(_cmd, default);

        // Assert
        await dbContext.Set<LivroModel>().Received()
            .AddAsync(
                Arg.Is<LivroModel>(x => x.Titulo == _cmd.Titulo),
                Arg.Any<CancellationToken>()
            );
        await dbContext.Received().SaveChangesAsync();
    }
}

