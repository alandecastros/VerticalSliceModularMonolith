using FluentValidation;

namespace VerticalSliceModularMonolith.Modules.Usuarios.Features.SalvarUsuario;

public class SalvarUsuarioCommandValidator : AbstractValidator<SalvarUsuarioCommand>
{
    public SalvarUsuarioCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotNull()
                .WithMessage("Nome é obrigatório");
    }
}