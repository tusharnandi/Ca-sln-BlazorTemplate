using System.Security.Claims;
using CaBlazorTemplate.Application.Common.Interfaces;

namespace CaBlazorTemplate.Server.Services;


public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    string? ICurrentUserService.UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
}

