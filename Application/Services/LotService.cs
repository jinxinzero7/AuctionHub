using Application.DTOs;
using Application.Mappings;
using Domain.Interfaces;
using Domain.Models;
using Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LotService : ILotService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILotRepository _lotRepository;
        private readonly ICurrentUserService _currentUserService;

        public LotService(ILotRepository lotRepository, ICurrentUserService currentUserService,
            IUserRepository userRepository)
        {
            _lotRepository = lotRepository;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
        }
        
        public async Task<Result<LotResponse>> CreateLotAsync(LotCreateRequest request)
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);

            var newLot = LotMappings.ToLot(request, currentUser);

            await _lotRepository.CreateLotAsync(newLot);

            return Result.Success<LotResponse>(newLot.ToLotResponse());
        }
        public async Task<Result<LotResponse>> GetLotByIdAsync(Guid id)
        {
            var lot = await _lotRepository.GetLotByIdAsync(id);
            return Result.Success<LotResponse>(lot.ToLotResponse());
        }
        public async Task<Result<IEnumerable<LotResponse>>> GetLotsByCreatorIdAsync(Guid creatorId)
        {
            var lots = await _lotRepository.GetLotsByCreatorIdAsync(creatorId);
            return Result.Success<IEnumerable<LotResponse>>(lots.ToLotResponse());
        }
        public async Task<Result<LotResponse>> UpdateLotByIdAsync(Guid id, LotUpdateRequest request)
        {
            var lot = await _lotRepository.GetLotByIdAsync(id);

            if (lot == null)
                return Result.Failure<LotResponse>("Ошибка: Лот не найден");

            if (lot.Bids != null && lot.Bids.Any())
                return Result.Failure<LotResponse>("Ошибка: Лот не может быть изменен если у него уже есть ставки");
           
            lot.Title = request.Title;
            lot.Description = request.Description;
            lot.StartingPrice = request.StartingPrice;
            lot.BidIncrement = request.BidIncrement;
            lot.EndDate = request.EndDate;
            lot.ImageUrl = request.ImageUrl;

            await _lotRepository.UpdateLotAsync(lot);
            
            return Result.Success<LotResponse>(lot.ToLotResponse());
        }
        public async Task<Result> DeleteLotAsync(Guid id)
        {
            var lot = await _lotRepository.GetLotByIdAsync(id);
            if (lot == null)
                return Result.Failure("Ошибка: Лот не найден");
            await _lotRepository.DeleteLotAsync(lot);
            return Result.Success();
        }
        
    }
}
