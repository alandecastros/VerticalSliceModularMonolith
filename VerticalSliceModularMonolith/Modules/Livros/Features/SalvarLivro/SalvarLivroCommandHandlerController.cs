using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.Modules.Livros.Features.SalvarLivro;

[Authorize]
[ApiController]
public class SalvarLivroCommandHandlerController : ControllerBase
{
    private readonly IUsuarioLogadoService _usuarioLogadoService;
    private readonly ICommandDispatcher _commandDispatcher;

    public SalvarLivroCommandHandlerController(IUsuarioLogadoService usuarioLogadoService, ICommandDispatcher commandDispatcher)
    {
        _usuarioLogadoService = usuarioLogadoService;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost("salvar-livro")]
    public async Task<IActionResult> Handle([FromBody] SalvarLivroCommand cmd)
    {
        cmd.Usuario = _usuarioLogadoService.UsuarioCodigo;

        await _commandDispatcher.Send(cmd);
        return Ok();
    }
}