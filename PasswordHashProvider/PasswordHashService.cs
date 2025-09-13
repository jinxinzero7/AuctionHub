using Domain.Interfaces.PassHasher;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace PasswordHashProvider
{
    public class PasswordHashService : IPasswordHashService
    {
        public string HashPassword(User user, string password)
        {
            var hashedPass = new PasswordHasher<User>().HashPassword(user, password);
            return hashedPass;
        }

        public bool VerifyHashedPassword(User user, string passwordHash, string password)
        {
            var result = new PasswordHasher<User>().VerifyHashedPassword(user, passwordHash, password);
            if (result == PasswordVerificationResult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
