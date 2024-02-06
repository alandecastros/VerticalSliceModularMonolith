using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.Modules.Usuarios.Features.SalvarUsuario;

[Authorize]
[ApiController]
public class SalvarUsuarioCommandHandlerController : ControllerBase
{
    private readonly IUsuarioLogadoService _usuarioLogadoService;
    private readonly ICommandDispatcher _commandDispatcher;

    public SalvarUsuarioCommandHandlerController(IUsuarioLogadoService usuarioLogadoService, ICommandDispatcher commandDispatcher)
    {
        _usuarioLogadoService = usuarioLogadoService;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost("salvar-usuario")]
    public async Task<IActionResult> Handle([FromBody] SalvarUsuarioCommand cmd)
    {
        cmd.Usuario = _usuarioLogadoService.UsuarioCodigo;

        await _commandDispatcher.Send(cmd);
        return Ok();
    }
}