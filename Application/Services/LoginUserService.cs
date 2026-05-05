using Application.Abstractions;
using Application.DTO;
using Application.DTOs;
using Application.Mappers;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Abstractions.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class LoginUserService(
    IUnitOfWork unitOfWork,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenService jwtTokenService)
    : ILoginUserService
{
    public async Task<TokenPairResponse> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken = default)
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
        var refresh = jwtTokenService.GenerateRefreshToken();
        var refreshHash = jwtTokenService.HashRefreshToken(refresh);
        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = refreshHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        await unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new TokenPairResponse(jwt, refresh);
    }
}