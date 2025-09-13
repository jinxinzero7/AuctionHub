using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.DTOInterfaces
{
    public interface IUserResponse
    {
        Guid Id { get; set; }
        string Username { get; set; }
        string Email { get; set; }
    }
}
