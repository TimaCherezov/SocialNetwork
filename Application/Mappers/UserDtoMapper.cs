using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers;

public static class UserDtoMapper
{
    public static UserResponse ToDto(User user) => new(user.Id, user.UserName, user.Email);
}

