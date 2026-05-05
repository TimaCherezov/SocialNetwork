using Application.DTOs;
using Domain.Abstractions;
using Application.Mappers;
using Application.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class GetUserService(IUnitOfWork unitOfWork) : IGetUserService
{
    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        return user;
    }
}
