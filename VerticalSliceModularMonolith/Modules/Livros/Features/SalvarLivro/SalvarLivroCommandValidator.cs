using FluentValidation;

namespace VerticalSliceModularMonolith.Modules.Livros.Features.SalvarLivro;

public class SalvarLivroCommandValidator : AbstractValidator<SalvarLivroCommand>
{
    public SalvarLivroCommandValidator()
    {
        RuleFor(x => x.Titulo)
            .NotNull()
                .WithMessage("Título é obrigatório");
    }
}