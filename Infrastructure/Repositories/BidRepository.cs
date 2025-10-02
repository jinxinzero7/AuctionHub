using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AuctionDbContext _context;

        public BidRepository(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task<Bid> GetBidByIdAsync(Guid id)
        {
            return await _context.Bids.FindAsync(id);
        }
        public async Task<IEnumerable<Bid>> GetBidsByLotIdAsync(Guid lotId)
        {
            return await _context.Bids
                .Where(b => b.LotId == lotId)
                .ToListAsync();
        }
        public async Task<Bid> CreateBidAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            return bid;
        }
        public async Task UpdateBidAsync(Bid bid)
        {
            _context.Bids.Update(bid);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteBidAsync(Bid bid)
        {
            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
        }
    }
}