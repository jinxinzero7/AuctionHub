using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IBidRepository
    {
        Task<Bid> GetBidByIdAsync(Guid lotId, Guid bidId);
        Task<IEnumerable<Bid>> GetBidsByLotIdAsync(Guid lotId);
        Task<Bid> CreateBidAsync(Bid bid);
        Task UpdateBidAsync(Bid bid);
        Task DeleteBidAsync(Bid bid);
    }
}