
using System.Threading;
using System.Threading.Tasks;
using Application.DTO;

namespace Application.Abstractions;

public interface IRefreshTokenService
{
    Task<TokenPairResponse> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default);
}

