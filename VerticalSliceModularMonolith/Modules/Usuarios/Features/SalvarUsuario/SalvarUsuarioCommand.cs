using MediatR;

namespace VerticalSliceModularMonolith.Modules.Usuarios.Features.SalvarUsuario;

public class SalvarUsuarioCommand : IRequest
{
    public string? Nome { get; set; }
    public string? Usuario { get; set; }
}
