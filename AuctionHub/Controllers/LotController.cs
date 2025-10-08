using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.DTOInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHub.Controllers
{
    [ApiController]
    [Route("api/v1/lots")]
    public class LotController : ControllerBase
    {
        private readonly ILotService _lotService;
        public LotController(ILotService lotService)
        {
            _lotService = lotService; 
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateLot([FromBody] LotCreateRequest request)
        {
            var result = await _lotService.CreateLotAsync(request);
            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetLotById), new { id = result.Value.Id }, result);
            return BadRequest(result.Error);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetLotById(Guid id)
        {
            var result = await _lotService.GetLotByIdAsync(id);
            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("creator/{creatorId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetLotsByCreatorId(Guid creatorId)
        {
            var result = await _lotService.GetLotsByCreatorIdAsync(creatorId);
            if (result.IsFailure)
                return NotFound();
            return Ok(result.Value);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateLot(Guid id, [FromBody] LotUpdateRequest request)
        {
            var result = await _lotService.UpdateLotByIdAsync(id, request);
            if (result.IsFailure)
                return NotFound(result.Error);
            return Ok(result.Value);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteLot(Guid id)
        {
            var result = await _lotService.DeleteLotAsync(id);
            if (result.IsFailure)
                return BadRequest(result.Error);
            return NoContent();
        }

    }
}
