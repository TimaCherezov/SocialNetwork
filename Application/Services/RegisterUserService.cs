using Application.Abstractions;
using Application.DTOs;
using Application.Mappers;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Events;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class RegisterUserService(
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher,
    IEventDispatcher eventDispatcher)
    : IRegisterUserService
{

    public async Task<UserResponse> RegisterAsync(
        RegisterUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        var isExisted = await unitOfWork.Users
            .GetByUserNameAsync(request.UserName, cancellationToken);
        if (isExisted != null)
        {
            throw new InvalidOperationException($"User {request.UserName} already exists.");
        }
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.UserName,
            FirstName = request.FirstName,
            SurName = request.SurName,
            Email = request.Email,
            Description = request.Description,
            Birthday = request.Birthday,
            CreatedAt = DateTime.UtcNow
        };
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        await unitOfWork.Users.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await eventDispatcher.DispatchAsync(
            new UserRegisteredDomainEvent(user.Id, user.UserName, user.Email, DateTime.UtcNow),
            cancellationToken);
        
        return UserDtoMapper.ToDto(user);
    }
}
