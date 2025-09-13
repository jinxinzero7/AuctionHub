using Domain.Interfaces.DTOInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<IUserResponse> RegisterAsync(IRegisterUserRequest request);
        Task<string> LoginAsync(ILoginUserRequest request);
        Task<IUserResponse> GetUserAsync(Guid userId);
        Task<IUserResponse> UpdateUserAsync(Guid userId, IUpdateUserRequest request);
        Task<IEnumerable<IUserResponse>> GetAllUsersAsync(); // Для админа
    }
}
