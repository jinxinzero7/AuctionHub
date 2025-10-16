using Domain.Interfaces.Jwt;
using Domain.Interfaces.PassHasher;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordHashProvider
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPasswordHashProvider(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHashService, PasswordHashService>();
            return services;
        }
    }
}
