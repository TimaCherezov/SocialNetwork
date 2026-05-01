using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

public static class UserDtoMapper
{
    public static UserResponse ToDto(User user, string jwtToken)
    {
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Token = jwtToken
        };
    }
}

