using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.DTOInterfaces;
using Domain.Models;
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
        
        public async Task<ILotResponse> CreateLotAsync(ILotCreateRequest request)
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);

            var newLot = new Lot
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                StartingPrice = request.StartingPrice,
                BidIncrement = request.BidIncrement,
                CurrentPrice = request.StartingPrice,
                StartDate = DateTime.UtcNow,
                EndDate = request.EndDate,
                ImageUrl = request.ImageUrl,
                IsCompleted = false,
                CreatorId = currentUserId,
                Creator = currentUser,
                WinnerId = null,
                Winner = null,
                Bids = new()
            };

            await _lotRepository.CreateLotAsync(newLot);

            var lotResponse = new LotResponse
            {
                Id = newLot.Id,
                Title = newLot.Title,
                Description = newLot.Description,
                StartingPrice = newLot.StartingPrice,
                BidIncrement = newLot.BidIncrement,
                CurrentPrice = newLot.CurrentPrice,
                StartDate = newLot.StartDate,
                EndDate = newLot.EndDate,
                ImageUrl = newLot.ImageUrl,
                IsCompleted = newLot.IsCompleted,
                Bids = newLot.Bids

            };

            return lotResponse;
        }
        public async Task<ILotResponse> GetLotByIdAsync(Guid id)
        {
            var lot = await _lotRepository.GetLotByIdAsync(id);
            return new LotResponse
            {
                Id = lot.Id,
                Title = lot.Title,
                Description = lot.Description,
                StartingPrice = lot.StartingPrice,
                BidIncrement = lot.BidIncrement,
                CurrentPrice = lot.CurrentPrice,
                StartDate = lot.StartDate,
                EndDate = lot.EndDate,
                ImageUrl = lot.ImageUrl,
                IsCompleted = lot.IsCompleted,
                Bids = lot.Bids
            };
        }
        public async Task<IEnumerable<ILotResponse>> GetLotsByCreatorIdAsync(Guid creatorId)
        {
            var lots = await _lotRepository.GetLotsByCreatorIdAsync(creatorId);
            return lots.Select(l => new LotResponse
            {
                Id = l.Id,
                Title = l.Title,
                Description = l.Description,
                StartingPrice = l.StartingPrice,
                BidIncrement = l.BidIncrement,
                CurrentPrice = l.CurrentPrice,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                ImageUrl = l.ImageUrl,
                IsCompleted = l.IsCompleted,
                Bids = l.Bids
            }).ToList();
        }
        public async Task<ILotResponse> UpdateLotByIdAsync(Guid id, ILotUpdateRequest request)
        {
            var lot = await _lotRepository.GetLotByIdAsync(id);
            if(lot.Bids != null && lot.Bids.Any())
            {
                throw new ArgumentException("A lot cannot be changed if it has bids");
            }

            lot.Title = request.Title;
            lot.Description = request.Description;
            lot.StartingPrice = request.StartingPrice;
            lot.BidIncrement = request.BidIncrement;
            lot.EndDate = request.EndDate;
            lot.ImageUrl = request.ImageUrl;

            await _lotRepository.UpdateLotAsync(lot);

            return new LotResponse
            {
                Id = lot.Id,
                Title = lot.Title,
                Description = lot.Description,
                StartingPrice = lot.StartingPrice,
                BidIncrement = lot.BidIncrement,
                CurrentPrice = lot.CurrentPrice,
                StartDate = lot.StartDate,
                EndDate = lot.EndDate,
                ImageUrl = lot.ImageUrl,
                IsCompleted = lot.IsCompleted,
                Bids = null
            };
        }
        public async Task<bool> DeleteLotAsync(Guid id)
        {
            var @lot = await _lotRepository.GetLotByIdAsync(id);
            await _lotRepository.DeleteLotAsync(lot);
            return true;
        }
        
    }
}
