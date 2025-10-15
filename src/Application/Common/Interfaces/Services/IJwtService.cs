namespace Application.Common.Interfaces.Services;
public interface IJwtService
{
    string GenerateToken(
        string role,
        string email,
        string id,
        string name,
        string surname);
}
