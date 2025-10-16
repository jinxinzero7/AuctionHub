using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LotRepository : ILotRepository
    {
        private readonly AuctionDbContext _context;

        public LotRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<Lot> GetLotByIdAsync(Guid id)
        {
            return await _context.Lots.FindAsync(id);
        }

        public async Task<IEnumerable<Lot>> GetLotsByCreatorIdAsync(Guid creatorId)
        {
            return await _context.Lots
                .Where(l => l.CreatorId == creatorId)
                .ToListAsync();
        }

        public async Task<Lot> CreateLotAsync(Lot lot)
        {
            _context.Lots.Add(lot);
            await _context.SaveChangesAsync();
            return lot;
        }

        public async Task UpdateLotAsync(Lot lot)
        {
            _context.Lots.Update(lot);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLotAsync(Lot lot)
        {
            _context.Lots.Remove(lot);
            await _context.SaveChangesAsync();
        }
    }
}
