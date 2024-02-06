using MediatR;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Modules.Usuarios.Abstractions;
using VerticalSliceModularMonolith.Shared.Exceptions;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.Modules.Usuarios.Features.SalvarUsuario;

public class SalvarUsuarioCommandHandler : IRequestHandler<SalvarUsuarioCommand>
{
    private readonly AppDbContext _dbContext;
    private readonly IUsuarioService _usuarioService;

    public SalvarUsuarioCommandHandler(
        AppDbContext dbContext,
        IUsuarioService usuarioService)
    {
        _dbContext = dbContext;
        _usuarioService = usuarioService;
    }

    public async Task Handle(SalvarUsuarioCommand request, CancellationToken cancellationToken)
    {
        if (await _usuarioService.ExisteAsync(request.Nome!, cancellationToken))
        {
            throw new BadRequestException("Usuário já existe com esse nome");
        }

        var usuario = new UsuarioModel
        {
            Nome = request.Nome!,
            CriadoPorCodigo = request.Usuario
        };

        await _dbContext.Set<UsuarioModel>().AddAsync(usuario, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
