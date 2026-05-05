using Application.DTO;
using Application.DTOs;

namespace Application.Abstractions;

public interface ILoginUserService
{
    Task<TokenPairResponse> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default);
}