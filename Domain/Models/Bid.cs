using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Bid
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public Guid LotId { get; set; }
        public Lot Lot { get; set; }
        public Guid BidderId { get; set; }
        public User Bidder { get; set; }
    }
}
