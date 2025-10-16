using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext!.User
                    .FindFirst("id")!.Value;

                return Guid.Parse(userId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to get user ID from claims", ex);
            }
        }

        public string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext!.User
                .FindFirst("userName")!.Value;
        }

        public string GetCurrentEmail()
        {
            return _httpContextAccessor.HttpContext!.User
                .FindFirst("email")!.Value;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?
                .Identity?.IsAuthenticated ?? false;
        }
    }
}
