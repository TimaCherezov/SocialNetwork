namespace Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string userName, string email);
}