using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Models;
using Domain.Results;

namespace Domain.Interfaces
{
    public interface IBidService
    {
        Task<Result<BidResponse>> CreateBidAsync(Guid lotId, BidCreateRequest request);
        Task<Result<BidResponse>> GetBidByIdAsync(Guid lotId, Guid bidId);
        Task<Result<IEnumerable<BidResponse>>> GetBidsByLotIdAsync(Guid lotId);
        Task<Result> DeleteBidByIdAsync(Guid lotId, Guid bidId);
    }
}