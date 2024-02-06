using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.GraphQL.TypeExtensions;

[ExtendObjectType(typeof(LivroModel))]
public class LivroModelTypeExtension
{
    public string DataCriacaoText([Parent] LivroModel livro)
    {
        return livro.DataCriacao.ToString("dd/MM/yyyy HH:mm");
    }
}