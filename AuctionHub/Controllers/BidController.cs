using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
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

            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetBidById), new { lotId = lotId, bidId = result.Value.Id }, result);
        }

        [HttpGet("{bidId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetBidById(Guid lotId, Guid bidId)
        {
            var result = await _bidService.GetBidByIdAsync(lotId, bidId);
            if (result.IsFailure)
                return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBids(Guid lotId)
        {
            var result = await _bidService.GetBidsByLotIdAsync(lotId);
            if (result.IsFailure)
                return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpDelete("{bidId:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteBid(Guid lotId, Guid bidId)
        {
            var result = await _bidService.DeleteBidByIdAsync(lotId, bidId);
            if (result.IsFailure)
                return BadRequest(result.Error);
            return Ok(result.IsSuccess);
        }
    }
}