using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface ILotRepository
    {
        Task<Lot> GetLotByIdAsync(Guid id);
        Task<IEnumerable<Lot>> GetLotsByCreatorIdAsync(Guid creatorId);
        Task<Lot> CreateLotAsync(Lot lot);
        Task UpdateLotAsync(Lot lot);
        Task DeleteLotAsync(Lot lot);
    }
}
