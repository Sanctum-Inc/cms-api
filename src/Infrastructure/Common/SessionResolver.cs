using System.Security.Claims;
using Application.Common.Interfaces.Session;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common;

public class SessionResolver : ISessionResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionResolver(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public string? Token
    {
        get
        {
            var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return null;
            }

            // Typically "Bearer <token>"
            var parts = authHeader.Split(' ');
            return parts.Length == 2 ? parts[1] : null;
        }
    }

    public string? UserId => GetClaim(ClaimTypes.NameIdentifier) ?? GetClaim("custom:user_id");
    public string? FirmId => GetClaim(ClaimTypes.UserData) ?? GetClaim("custom:firm_id");

    public string? UserEmail => GetClaim(ClaimTypes.Email) ?? GetClaim("email");

    private string? GetClaim(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;
    }
}
