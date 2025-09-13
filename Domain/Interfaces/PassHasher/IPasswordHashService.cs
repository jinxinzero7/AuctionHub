using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.PassHasher
{
    public interface IPasswordHashService
    {
        string HashPassword(User user, string password);
        bool VerifyHashedPassword(User user, string passwordHash, string password);
    }
}
