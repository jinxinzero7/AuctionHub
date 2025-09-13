using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.DTOInterfaces
{
    public interface IUpdateUserRequest
    {
        string? Username { get; set; }
        string? Password { get; set; }
        string? Email { get; set; }
    }
}
