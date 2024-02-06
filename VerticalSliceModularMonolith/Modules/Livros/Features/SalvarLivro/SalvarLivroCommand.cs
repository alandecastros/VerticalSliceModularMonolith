using MediatR;

namespace VerticalSliceModularMonolith.Modules.Livros.Features.SalvarLivro;

public class SalvarLivroCommand : IRequest
{
    public string? Titulo { get; set; }
    public string? Usuario { get; set; }
}
