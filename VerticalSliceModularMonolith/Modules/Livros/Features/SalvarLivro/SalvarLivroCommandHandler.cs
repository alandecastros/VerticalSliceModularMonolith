using MediatR;
using VerticalSliceModularMonolith.Infrastructure.Database;
using VerticalSliceModularMonolith.Modules.Livros.Abstractions;
using VerticalSliceModularMonolith.Shared.Exceptions;
using VerticalSliceModularMonolith.Shared.Models;

namespace VerticalSliceModularMonolith.Modules.Livros.Features.SalvarLivro;

public class SalvarLivroCommandHandler : IRequestHandler<SalvarLivroCommand>
{
    private readonly AppDbContext _dbContext;
    private readonly ILivroService _livroService;

    public SalvarLivroCommandHandler(
        AppDbContext dbContext,
        ILivroService livroService)
    {
        _dbContext = dbContext;
        _livroService = livroService;
    }

    public async Task Handle(SalvarLivroCommand request, CancellationToken cancellationToken)
    {
        if (await _livroService.ExisteAsync(request.Titulo!, cancellationToken))
        {
            throw new BadRequestException("Livro já existe com esse título");
        }

        var livro = new LivroModel
        {
            Titulo = request.Titulo!,
            CriadoPorCodigo = request.Usuario!
        };

        await _dbContext.Set<LivroModel>().AddAsync(livro, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
