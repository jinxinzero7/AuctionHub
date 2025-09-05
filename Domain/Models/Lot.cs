using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Lot
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal BidIncrement { get; set; } // Шаг ставки
        public decimal? CurrentPrice { get; set; } // Можно вычислять, но проще хранить
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsCompleted { get; set; } = false;

        public Guid CreatorId { get; set; }
        public User Creator { get; set; }
        public Guid? WinnerId { get; set; }
        public User? Winner { get; set; }
        public List<Bid> Bids { get; set; } = new();
    }
}
