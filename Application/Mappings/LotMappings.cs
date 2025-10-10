using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Models;
using Domain.Interfaces.DTOInterfaces;

namespace Application.Mappings
{
    public static class LotMappings
    {
        public static LotResponse ToLotResponse(this Lot lot)
        {
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

        public static IEnumerable<LotResponse> ToLotResponse(this IEnumerable<Lot> lots)
        {
            return lots.Select(lots => lots.ToLotResponse()).ToList();
        }

        public static Lot ToLot(ILotCreateRequest request, User currentUser)
        {
            return new Lot
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
                Creator = currentUser,
                CreatorId = currentUser.Id,
                WinnerId = null,
                Winner = null,
                Bids = new()
            };
        }
    }
}