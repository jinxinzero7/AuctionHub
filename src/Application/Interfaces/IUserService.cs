using Application.DTOs;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponse>> RegisterAsync(RegisterUserRequest request);
        Task<Result<string>> LoginAsync(LoginUserRequest request);
        Task<Result<UserResponse>> GetUserAsync(Guid userId);
        Task<Result<UserResponse>> UpdateUserAsync(Guid userId, UpdateUserRequest request);
        Task<Result<IEnumerable<UserResponse>>> GetAllUsersAsync(); // Для админа
    }
}
