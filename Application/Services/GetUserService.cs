using Application.DTOs;
using Domain.Abstractions;
using Application.Mappers;
using Application.Abstractions;
using Domain.Entities;

namespace Application.Services;

public class GetUserService : IGetUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);
        return user;
    }
}
