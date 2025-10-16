using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Domain.Interfaces
{
    public interface ILotService
    {
        Task<Result<LotResponse>> CreateLotAsync(LotCreateRequest request);
        Task<Result<LotResponse>> GetLotByIdAsync(Guid id);
        Task<Result<IEnumerable<LotResponse>>> GetLotsByCreatorIdAsync(Guid creatorId);
        Task<Result<LotResponse>> UpdateLotByIdAsync(Guid id, LotUpdateRequest request);
        Task<Result> DeleteLotAsync(Guid id);
    }
}
