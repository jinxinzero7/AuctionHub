using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces.DTOInterfaces;
using Domain.Results;

namespace Domain.Interfaces
{
    public interface IBidService
    {
        Task<Result<IBidResponse>> CreateBidAsync(Guid lotId, IBidCreateRequest request);
        Task<Result<IBidResponse>> GetBidByIdAsync(Guid lotId, Guid bidId);
        Task<Result<IEnumerable<IBidResponse>>> GetBidsByLotIdAsync(Guid lotId);
        Task<Result> DeleteBidByIdAsync(Guid lotId, Guid bidId);
    }
}