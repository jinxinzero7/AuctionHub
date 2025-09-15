using Application.Services;
using Domain.Interfaces;
using Domain.Interfaces.Jwt;
using Domain.Interfaces.PassHasher;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<ILotService, LotService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILotRepository, LotRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ILotService, LotService>();
            return services;
        }
    }
}
