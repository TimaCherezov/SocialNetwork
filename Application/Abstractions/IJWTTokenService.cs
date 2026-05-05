namespace Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string userName, string email);
    string GenerateRefreshToken();
    string HashRefreshToken(string token);
}