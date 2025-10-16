using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BidResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid BidderId { get; set; }
    }
}