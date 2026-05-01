using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Abstractions;

public interface IRegisterUserService
{
    Task<UserResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
}
