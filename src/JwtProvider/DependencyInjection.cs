using Domain.Interfaces;
using Domain.Interfaces.Jwt;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtProvider
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtProvider(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
