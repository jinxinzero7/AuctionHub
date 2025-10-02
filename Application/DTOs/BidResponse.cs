using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.DTOInterfaces;

namespace Application.DTOs
{
    public class BidResponse : IBidResponse
    {
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid BidderId { get; set; }
    }
}