using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Interfaces.DTOInterfaces;
using Domain.Models;
using Application.DTOs;
using Microsoft.EntityFrameworkCore.Storage;

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

        public async Task<IBidResponse> CreateBidAsync(Guid lotId, IBidCreateRequest request)
        {
            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentUser = await _userRepository.GetUserByIdAsync(currentUserId);
            var currentLot = await _lotRepository.GetLotByIdAsync(lotId);

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

            await _bidRepository.CreateBidAsync(newBid);

            var bidResponse = new BidResponse
            {
                Amount = newBid.Amount,
                Timestamp = newBid.Timestamp,
                BidderId = newBid.BidderId
            };

            return bidResponse;
        }
        public async Task<IBidResponse> GetBidByIdAsync(Guid id)
        {
            var bid = await _bidRepository.GetBidByIdAsync(id);

            var bidResponse = new BidResponse
            {
                Amount = bid.Amount,
                Timestamp = bid.Timestamp,
                BidderId = bid.BidderId
            };
            return bidResponse;
        }
        public async Task<IEnumerable<IBidResponse>> GetBidsByLotIdAsync(Guid lotId)
        {
            var bids = await _bidRepository.GetBidsByLotIdAsync(lotId);
            return bids.Select(b => new BidResponse
            {
                Amount = b.Amount,
                Timestamp = b.Timestamp,
                BidderId = b.BidderId
            }).ToList();
        }
        public async Task<bool> DeleteBidByIdAsync(Guid id)
        {
            var bid = await _bidRepository.GetBidByIdAsync(id);
            await _bidRepository.DeleteBidAsync(bid);
            return true;
        }
    }
}