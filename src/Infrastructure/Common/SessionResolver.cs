using System.IdentityModel.Tokens.Jwt;
using Application.Common.Interfaces.Session;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common;

public class SessionResolver(IHttpContextAccessor httpContextAccessor) : ISessionResolver
{
    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public string? Token
    {
        get
        {
            var authHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader))
            {
                return null;
            }

            // Typically "Bearer <token>"
            var parts = authHeader.Split(' ');
            return parts.Length == 2 ? parts[1] : null;
        }
    }

    public string? UserId => GetClaim(JwtRegisteredClaimNames.Sid) ?? GetClaim("sid");
    public string? FirmId => GetClaim("firmId");

    public string? UserEmail => GetClaim(JwtRegisteredClaimNames.Email) ?? GetClaim("email");

    private string? GetClaim(string claimType)
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(claimType)?.Value;
    }
}
