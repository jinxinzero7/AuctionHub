using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Models;

namespace Application.Mappings
{
    public static class BidMappings
    {
        public static BidResponse ToBidResponse(this Bid bid)
        {
            return new BidResponse
            {
                Id = bid.Id,
                Amount = bid.Amount,
                Timestamp = bid.Timestamp,
                BidderId = bid.BidderId
            };
        }

        public static IEnumerable<BidResponse> ToBidResponse(this IEnumerable<Bid> bids)
        {
            // Sort by descending (amount)
            return bids.Select(bids => bids.ToBidResponse()).OrderByDescending(b => b.Amount).ToList();
        }

        public static Bid ToBid(BidCreateRequest request, Lot currentLot, User currentUser)
        {
            return new Bid
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Timestamp = DateTime.UtcNow,
                Lot = currentLot,
                BidderId = currentUser.Id,
                Bidder = currentUser
            };
        }
    }
}