using System.Security.Claims;
using VerticalSliceModularMonolith.Shared.Abstractions;

namespace VerticalSliceModularMonolith.Shared.Services;

public class UsuarioLogadoService : IUsuarioLogadoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioLogadoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UsuarioCodigo
    {
        get
        {
            var claimsIdentity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
            var codigoUsuario = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
            return codigoUsuario;
        }
    }
}