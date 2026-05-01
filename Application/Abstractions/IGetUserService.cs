
using Domain.Entities;

namespace Application.Abstractions;

public interface IGetUserService
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

