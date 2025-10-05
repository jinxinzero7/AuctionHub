using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.DTOInterfaces;

namespace Domain.Interfaces
{
    public interface IBidService
    {
        Task<IBidResponse> CreateBidAsync(Guid lotId, IBidCreateRequest request);
        Task<IBidResponse> GetBidByIdAsync(Guid lotId, Guid bidId);
        Task<IEnumerable<IBidResponse>> GetBidsByLotIdAsync(Guid lotId);
        Task<bool> DeleteBidByIdAsync(Guid lotId, Guid bidId);
    }
}