using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICurrentUserService
    {
        Guid GetCurrentUserId();
        string GetCurrentUsername();
        string GetCurrentEmail();
        bool IsAuthenticated();
    }
}
