using Application.DTOs;

namespace Application.Abstractions;

public interface ILoginUserService
{
    Task<UserResponse> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default);
}