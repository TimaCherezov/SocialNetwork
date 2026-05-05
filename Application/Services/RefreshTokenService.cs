using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions;
using Application.DTO;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class RefreshTokenService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService) : IRefreshTokenService
{
    public async Task<TokenPairResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenHash = jwtTokenService.HashRefreshToken(refreshToken);
        var currentToken = await unitOfWork.RefreshTokens.GetByTokenHashAsync(tokenHash, cancellationToken);
        if (currentToken == null || currentToken.RevokedAt != null || currentToken.ExpiresAt <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid or expired refresh token");

        var user = await unitOfWork.Users.GetByIdAsync(currentToken.UserId, cancellationToken);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        var newAccess = jwtTokenService.GenerateToken(user.Id, user.UserName, user.Email);
        var newRefresh = jwtTokenService.GenerateRefreshToken();
        var newRefreshHash = jwtTokenService.HashRefreshToken(newRefresh);

        currentToken.RevokedAt = DateTime.UtcNow;
        await unitOfWork.RefreshTokens.UpdateAsync(currentToken, cancellationToken);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = newRefreshHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7) 
        };
        await unitOfWork.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TokenPairResponse(newAccess, newRefresh);
    }
}
