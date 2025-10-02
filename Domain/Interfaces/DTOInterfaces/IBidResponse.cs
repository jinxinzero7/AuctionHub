using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces.DTOInterfaces
{
    public interface IBidResponse
    {
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid BidderId { get; set; }
    }
}