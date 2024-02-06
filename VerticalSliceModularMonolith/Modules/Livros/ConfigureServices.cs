using VerticalSliceModularMonolith.Modules.Livros.Abstractions;
using VerticalSliceModularMonolith.Modules.Livros.Services;

namespace VerticalSliceModularMonolith.Modules.Livros;

public static class ConfigureServices
{
    public static void AddLivros(this IServiceCollection services)
    {
        services.AddScoped<ILivroService, LivroService>();
    }
}
