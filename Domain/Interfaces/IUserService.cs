using Domain.Interfaces.DTOInterfaces;
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
        Task<Result<IUserResponse>> RegisterAsync(IRegisterUserRequest request);
        Task<Result<string>> LoginAsync(ILoginUserRequest request);
        Task<Result<IUserResponse>> GetUserAsync(Guid userId);
        Task<Result<IUserResponse>> UpdateUserAsync(Guid userId, IUpdateUserRequest request);
        Task<Result<IEnumerable<IUserResponse>>> GetAllUsersAsync(); // Для админа
    }
}
