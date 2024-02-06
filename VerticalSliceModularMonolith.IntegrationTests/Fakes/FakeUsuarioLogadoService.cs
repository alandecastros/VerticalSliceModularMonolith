using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.IntegrationTests.Fakes;
public static class UsuarioLogado
{
    public static string? Codigo { get; set; }
}

public class FakeUsuarioLogadoService : IUsuarioLogadoService
{
    public string? UsuarioCodigo => UsuarioLogado.Codigo;
}
