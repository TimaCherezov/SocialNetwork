using Application.Abstractions;
using Application.DTOs;
using Application.Mappers;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class RegisterUserService : IRegisterUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterUserService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<UserResponse> RegisterAsync(
        RegisterUserRequest request, 
        CancellationToken cancellationToken = default)
    {
        var isExisted = await _unitOfWork.Users
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
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var jwt = _jwtTokenService.GenerateToken(user.Id, user.UserName, user.UserName);
        return UserDtoMapper.ToDto(user, jwt);
    }
}
