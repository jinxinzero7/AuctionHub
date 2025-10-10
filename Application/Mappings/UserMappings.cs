using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Models;
using Domain.Interfaces.DTOInterfaces;

namespace Application.Mappings
{
    public static class UserMappings
    {
        public static UserResponse ToUserResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username
            };
        }

        public static IEnumerable<UserResponse> ToUserResponse(this IEnumerable<User> users)
        {
            return users.Select(user => user.ToUserResponse()).ToList();
        }

        public static User ToUser(IRegisterUserRequest request)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Username = request.Username,
            };
        }
    }
}