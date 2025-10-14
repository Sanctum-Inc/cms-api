namespace Application.Common.Interfaces.Session;
public interface ISessionResolver
{
    string? UserId { get; }
    string? UserEmail { get; }
    bool IsAuthenticated { get; }
    string? Token { get; }
}
