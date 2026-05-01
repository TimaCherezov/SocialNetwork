using Application.Abstractions;
using Application.DTOs;
using Application.Mappers;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class LoginUserService(
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService jwtTokenService)
    : ILoginUserService
{
    public async Task<UserResponse> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("Invalid email");
        }
        
        var result  = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidOperationException("Invalid Password");
        }
        var jwt = jwtTokenService.GenerateToken(user.Id, user.UserName, user.Email);
        return UserDtoMapper.ToDto(user, jwt);
    }
}