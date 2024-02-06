using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Modules.Usuarios.Abstractions;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.Modules.Usuarios.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _dbContext;

    public UsuarioService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExisteAsync(string nome, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LivroModel>()
                    .AnyAsync(x => x.Titulo == nome, cancellationToken);
    }
}