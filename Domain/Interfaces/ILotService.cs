using Domain.Interfaces.DTOInterfaces;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILotService
    {
        Task<Result<ILotResponse>> CreateLotAsync(ILotCreateRequest request);
        Task<Result<ILotResponse>> GetLotByIdAsync(Guid id);
        Task<Result<IEnumerable<ILotResponse>>> GetLotsByCreatorIdAsync(Guid creatorId);
        Task<Result<ILotResponse>> UpdateLotByIdAsync(Guid id, ILotUpdateRequest request);
        Task<Result> DeleteLotAsync(Guid id);
    }
}
