using Domain.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
