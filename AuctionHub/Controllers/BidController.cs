using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Domain.Interfaces.DTOInterfaces;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace AuctionHub.Controllers
{
    [ApiController]
    [Route("api/v1/lots/{lotId:guid}/bids")]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBid(Guid lotId, [FromBody] BidCreateRequest request)
        {
            var result = await _bidService.CreateBidAsync(lotId, request);
            return CreatedAtAction(nameof(GetBidById), new { lotId = lotId, bidId = result.Id }, result);
        }

        [HttpGet("{bidId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetBidById(Guid lotId, Guid bidId)
        {
            var bid = await _bidService.GetBidByIdAsync(lotId, bidId);
            if (bid == null)
                return NotFound();
            return Ok(bid);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBids(Guid lotId)
        {
            var bids = await _bidService.GetBidsByLotIdAsync(lotId);
            if (bids == null)
                return NotFound();
            return Ok(bids);
        }

        [HttpDelete("{bidId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteBid(Guid lotId, Guid bidId)
        {
            var result = await _bidService.DeleteBidByIdAsync(lotId, bidId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}