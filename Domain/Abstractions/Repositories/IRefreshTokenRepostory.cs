using Domain.Entities;

namespace Domain.Abstractions.Repositories;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
public interface IRefreshTokenRepostory
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);

    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task RemoveAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task<IEnumerable<RefreshToken>> GetActiveByUserIdAsync(Guid userId, DateTime nowUtc,
        CancellationToken cancellationToken = default);
}