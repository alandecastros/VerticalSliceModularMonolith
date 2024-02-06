using VerticalSliceModularMonolith.Modules.Usuarios.Abstractions;
using VerticalSliceModularMonolith.Modules.Usuarios.Services;

namespace VerticalSliceModularMonolith.Modules.Usuarios;

public static class ConfigureServices
{
    public static void AddUsuarios(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioService, UsuarioService>();
    }
}
