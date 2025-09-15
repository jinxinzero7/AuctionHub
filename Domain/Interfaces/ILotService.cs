using Domain.Interfaces.DTOInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILotService
    {
        Task<ILotResponse> CreateLotAsync(ILotCreateRequest request);
        Task<ILotResponse> GetLotByIdAsync(Guid id);
        Task<IEnumerable<ILotResponse>> GetLotsByCreatorIdAsync(Guid creatorId);
        Task<ILotResponse> UpdateLotByIdAsync(Guid id, ILotUpdateRequest request);
        Task<bool> DeleteLotAsync(Guid id);
    }
}
