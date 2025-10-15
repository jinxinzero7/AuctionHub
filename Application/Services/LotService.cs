using Application.DTOs;
using Application.Mappings;
using Domain.Interfaces;
using Domain.Models;
using Domain.Results;
using FluentValidation;
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
        private readonly IValidator<LotCreateRequest> _createLotValidator;
        private readonly IValidator<LotUpdateRequest> _updateLotValidator;

        public LotService(ILotRepository lotRepository,
            ICurrentUserService currentUserService,
            IUserRepository userRepository,
            IValidator<LotCreateRequest> createLotValidator,
            IValidator<LotUpdateRequest> updateLotValidator)
        {
            _lotRepository = lotRepository;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _createLotValidator = createLotValidator;
            _updateLotValidator = updateLotValidator;
        }
        
        public async Task<Result<LotResponse>> CreateLotAsync(LotCreateRequest request)
        {
            // Валидация
            var validationResult = await _createLotValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<LotResponse>($"Validation failed: {errors}");
            }

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
            // Валидация
            var validationResult = await _updateLotValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return Result.Failure<LotResponse>($"Validation failed: {errors}");
            }

            var lot = await _lotRepository.GetLotByIdAsync(id);

            if (lot == null)
                return Result.Failure<LotResponse>("Ошибка: Лот не найден");

            if (lot.Bids != null && lot.Bids.Any())
                return Result.Failure<LotResponse>("Ошибка: Лот не может быть изменен если у него уже есть ставки");

            // Обновляем только те поля, которые предоставлены в request          
            if (!string.IsNullOrEmpty(request.Title))
                lot.Title = request.Title; // Изменяем поле, если оно не пустое
                
            if (!string.IsNullOrEmpty(request.Description))
                lot.Description = request.Description;
                
            if (request.StartingPrice > 0)
                lot.StartingPrice = request.StartingPrice;
                
            if (request.BidIncrement > 0)
                lot.BidIncrement = request.BidIncrement;
                
            if (request.EndDate != default)
                lot.EndDate = request.EndDate;
                
            if (!string.IsNullOrEmpty(request.ImageUrl))
                lot.ImageUrl = request.ImageUrl;

            await _lotRepository.UpdateLotAsync(lot);
            
            return Result.Success<LotResponse>(lot.ToLotResponse());
        }
        public async Task<Result> DeleteLotAsync(Guid id)
        {
            var lot = await _lotRepository.GetLotByIdAsync(id);
            if (lot == null)
                return Result.Failure("Ошибка: Лот не найден");
            if (lot.Bids != null && lot.Bids.Any())
                return Result.Failure<LotResponse>("Ошибка: Лот не может быть удален, если у него уже есть ставки");
            await _lotRepository.DeleteLotAsync(lot);
            return Result.Success();
        }
        
    }
}
