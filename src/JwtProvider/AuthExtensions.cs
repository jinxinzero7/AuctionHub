using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtProvider
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, 
            IConfiguration configuration)
        {
            var authSettings = configuration.GetSection(nameof(AuthSettings))
                .Get<AuthSettings>();

            if (authSettings == null)
                throw new ArgumentNullException(nameof(AuthSettings),
                    "AuthSettings не найдены в конфигурации");

            if (string.IsNullOrEmpty(authSettings.SecretKey))
                throw new ArgumentException("SecretKey не может быть пустым");

            if (string.IsNullOrEmpty(authSettings.Issuer))
                throw new ArgumentException("Issuer не может быть пустым");

            if (string.IsNullOrEmpty(authSettings.Audience))
                throw new ArgumentException("Audience не может быть пустым");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authSettings.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(authSettings.SecretKey))
                    };
                });

            return services;
                
        }
    }
}
