using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.GraphQL.TypeExtensions;

[ExtendObjectType(typeof(UsuarioModel))]
public class UsuarioModelTypeExtension
{
    public string DataCriacaoText([Parent] UsuarioModel usuario)
    {
        return usuario.DataCriacao.ToString("dd/MM/yyyy HH:mm");
    }
}
