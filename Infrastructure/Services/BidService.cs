using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.DTOInterfaces;
using Domain.Models;
using Application.DTOs;
using Microsoft.EntityFrameworkCore.Storage;
using Domain.Results;

namespace Infrastructure.Services
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

        public async Task<Result<IBidResponse>> CreateBidAsync(Guid lotId, IBidCreateRequest request)
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
            var currentLot = await _lotRepository.GetLotByIdAsync(lotId);

            if (currentLot == null)
                return Result.Failure<IBidResponse>("Ошибка: Лот не найден"); // как надо

            if (currentLot.IsCompleted || currentLot.EndDate < DateTime.UtcNow)
                return Result.Failure<IBidResponse>("Ошибка: Аукцион завершен"); // как было

            if (currentLot.CreatorId == currentUserId)
                return Result.Failure<IBidResponse>("Ошибка: Нельзя делать ставки на свои лоты");

            var currentPrice = currentLot.CurrentPrice ?? currentLot.StartingPrice;
            var minimumBid = currentPrice + currentLot.BidIncrement;
        
            if (request.Amount < minimumBid)
                return Result.Failure<IBidResponse>($"Ошибка: Минимальная ставка: {minimumBid}");

            var newBid = new Bid
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Timestamp = DateTime.UtcNow,
                LotId = lotId,
                Lot = currentLot,
                BidderId = currentUserId,
                Bidder = currentUser
            };

            currentLot.CurrentPrice = request.Amount;
            await _lotRepository.UpdateLotAsync(currentLot);

            await _bidRepository.CreateBidAsync(newBid);

            var bidResponse = new BidResponse
            {
                Id = newBid.Id,
                Amount = newBid.Amount,
                Timestamp = newBid.Timestamp,
                BidderId = newBid.BidderId
            };

            return Result.Success<IBidResponse>(bidResponse);
        }
        public async Task<Result<IBidResponse>> GetBidByIdAsync(Guid lotId, Guid bidId)
        {
            var bid = await _bidRepository.GetBidByIdAsync(lotId, bidId);

            if(bid == null)
                return Result.Failure<IBidResponse>("Ошибка: Ставка не найдена");

            var bidResponse = new BidResponse
            {
                Id = bid.Id,
                Amount = bid.Amount,
                Timestamp = bid.Timestamp,
                BidderId = bid.BidderId
            };
            return Result.Success<IBidResponse>(bidResponse);
        }
        public async Task<Result<IEnumerable<IBidResponse>>> GetBidsByLotIdAsync(Guid lotId)
        {
            var bids = await _bidRepository.GetBidsByLotIdAsync(lotId);

            if(bids == null)
                return Result.Failure<IEnumerable<IBidResponse>>("Ставки по данному лоту не найдены");

            var bidsResponse = bids.Select(b => new BidResponse
            {
                Id = b.Id,
                Amount = b.Amount,
                Timestamp = b.Timestamp,
                BidderId = b.BidderId
            }).OrderByDescending(b => b.Amount).ToList();

            return Result.Success<IEnumerable<IBidResponse>>(bidsResponse);
        }
        public async Task<Result> DeleteBidByIdAsync(Guid lotId, Guid bidId)
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var bid = await _bidRepository.GetBidByIdAsync(lotId, bidId);

            if (bid == null)
                return Result.Failure("Ошибка: Ставка не найдена");

            if (currentUserId != bid.BidderId)
                    return Result.Failure("Ошибка: Вы не являетесь владельцем ставки и поэтому не можете ее удалить");
            
            await _bidRepository.DeleteBidAsync(bid);
            return Result.Success();
        }
    }
}