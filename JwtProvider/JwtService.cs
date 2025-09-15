using Domain.Interfaces.Jwt;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtProvider
{
    public class JwtService(IOptions<AuthSettings> options) : IJwtService
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("userName", user.Username ?? throw new ArgumentNullException("Username is required")),
                new Claim("email", user.Email ?? throw new ArgumentNullException("Email is required")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var jwtToken = new JwtSecurityToken(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
                
        }
    }
}
