namespace VerticalSliceModularMonolith.Modules.Livros.Abstractions;

public interface ILivroService
{
    Task<bool> ExisteAsync(string titulo, CancellationToken cancellationToken = default);
}