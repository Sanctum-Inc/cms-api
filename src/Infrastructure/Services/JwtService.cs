using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces.Services;
using Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace test;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
    }

    public string GenerateToken(
        string role,
        string email,
        string id,
        string name,
        string surname,
        string firmId,
        int version,
        bool isEmailVerification,
        string? expiry = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sid, id),
            new Claim(JwtRegisteredClaimNames.Name, name),
            new Claim(JwtRegisteredClaimNames.FamilyName, surname),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("version", version.ToString()),
            new Claim("purpose", isEmailVerification ? "email_verification" : "access"),
            new Claim("roles", role),
            new Claim("firmId", firmId)
        };

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(expiry ?? _jwtOptions.ExpireMinutes)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<TokenValidationResult> VerifyToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key!));
        var result = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, new()
        {
            IssuerSigningKey = key,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            ClockSkew = TimeSpan.Zero,
         });

        return result;
    }
}
