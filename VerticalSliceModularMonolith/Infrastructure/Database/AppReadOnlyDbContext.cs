using Microsoft.EntityFrameworkCore;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.Infrastructure.Database;

public class AppReadOnlyDbContext : DbContext
{
    public AppReadOnlyDbContext(
        DbContextOptions<AppReadOnlyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioModelConfig).Assembly);
    }
}