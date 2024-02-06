using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Modules.Livros.Abstractions;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.Modules.Livros.Services;

public class LivroService : ILivroService
{
    private readonly AppDbContext _dbContext;

    public LivroService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExisteAsync(string titulo, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<LivroModel>()
                    .AnyAsync(x => x.Titulo == titulo, cancellationToken);
    }
}
