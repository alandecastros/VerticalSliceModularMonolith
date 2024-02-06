namespace VerticalSliceModularMonolith.Modules.Usuarios.Abstractions;

public interface IUsuarioService
{
    Task<bool> ExisteAsync(string nome, CancellationToken cancellationToken = default);
}
