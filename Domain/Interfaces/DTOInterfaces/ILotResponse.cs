using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.DTOInterfaces
{
    public interface ILotResponse
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        decimal StartingPrice { get; set; }
        decimal BidIncrement { get; set; }
        decimal? CurrentPrice { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string? ImageUrl { get; set; }
        bool IsCompleted { get; set; }
        List<Bid> Bids { get; set; }
    }
}
