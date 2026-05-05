using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class RefreshTokenRepository(AppDbContext dbContext) : IRefreshTokenRepostory
{
    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        => await dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash, cancellationToken);

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        => await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);

    public  Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        dbContext.RefreshTokens.Update(refreshToken);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        dbContext.RefreshTokens.Remove(refreshToken);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, DateTime nowUtc, CancellationToken cancellationToken = default)
        => await dbContext.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.ExpiresAt > nowUtc && rt.RevokedAt == null)
            .ToListAsync(cancellationToken);
        
}