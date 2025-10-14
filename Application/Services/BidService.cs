using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Application.DTOs;
using Domain.Results;
using Application.Mappings;
using Domain.Enums;

namespace Application.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserRepository _userRepository;
        private readonly ILotRepository _lotRepository;

        public BidService(IBidRepository bidRepository, ICurrentUserService currentUserService,
        IUserRepository userRepository, ILotRepository lotRepository)
        {
            _bidRepository = bidRepository;
            _currentUserService = currentUserService;
            _userRepository = userRepository;
            _lotRepository = lotRepository;
        }

        public async Task<Result<BidResponse>> CreateBidAsync(Guid lotId, BidCreateRequest request)
        {
            // Get Current Entities
            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
            var currentLot = await _lotRepository.GetLotByIdAsync(lotId);

            // Validation
            if (currentLot == null)
                return Result.Failure<BidResponse>("Ошибка: Лот не найден", ErrorType.NotFound); 

            if (currentLot.IsCompleted || currentLot.EndDate < DateTime.UtcNow)
                return Result.Failure<BidResponse>("Ошибка: Аукцион завершен");

            if (currentLot.CreatorId == currentUserId)
                return Result.Failure<BidResponse>("Ошибка: Нельзя делать ставки на свои лоты");

            var currentPrice = currentLot.CurrentPrice ?? currentLot.StartingPrice; // Calculationg current price 
            var minimumBid = currentPrice + currentLot.BidIncrement;

            if (request.Amount < minimumBid)
                return Result.Failure<BidResponse>($"Ошибка: Минимальная ставка: {minimumBid}");
                
            // Mapping
            var newBid = BidMappings.ToBid(request, currentLot, currentUser);

            // Change current price to actual price
            currentLot.CurrentPrice = request.Amount;
            await _lotRepository.UpdateLotAsync(currentLot);

            await _bidRepository.CreateBidAsync(newBid);

            return Result.Success<BidResponse>(newBid.ToBidResponse());
        }
        public async Task<Result<BidResponse>> GetBidByIdAsync(Guid lotId, Guid bidId)
        {
            var bid = await _bidRepository.GetBidByIdAsync(lotId, bidId);

            if(bid == null)
                return Result.Failure<BidResponse>("Ошибка: Ставка не найдена", ErrorType.NotFound);

            return Result.Success<BidResponse>(bid.ToBidResponse());
        }
        public async Task<Result<IEnumerable<BidResponse>>> GetBidsByLotIdAsync(Guid lotId)
        {
            var bids = await _bidRepository.GetBidsByLotIdAsync(lotId);

            if(bids == null)
                return Result.Failure<IEnumerable<BidResponse>>("Ставки по данному лоту не найдены", ErrorType.NotFound);

            return Result.Success<IEnumerable<BidResponse>>(bids.ToBidResponse());
        }
        public async Task<Result> DeleteBidByIdAsync(Guid lotId, Guid bidId)
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var bid = await _bidRepository.GetBidByIdAsync(lotId, bidId);

            if (bid == null)
                return Result.Failure("Ошибка: Ставка не найдена", ErrorType.NotFound);

            if (currentUserId != bid.BidderId)
                    return Result.Failure("Ошибка: Вы не являетесь владельцем ставки и поэтому не можете ее удалить");
            
            await _bidRepository.DeleteBidAsync(bid);
            return Result.Success();
        }
    }
}