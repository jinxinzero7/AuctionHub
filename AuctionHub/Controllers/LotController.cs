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
            return CreatedAtAction(nameof(GetLotById), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetLotById(Guid id)
        {
            var lot = await _lotService.GetLotByIdAsync(id);
            if (lot == null)
                return NotFound();

            return Ok(lot);
        }

        [HttpGet("creator/{creatorId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetLotsByCreatorId(Guid creatorId)
        {
            var lots = await _lotService.GetLotsByCreatorIdAsync(creatorId);
            if (lots == null || !lots.Any())
                return NotFound();

            return Ok(lots);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateLot(Guid id, [FromBody] LotUpdateRequest request)
        {
            var lot = await _lotService.UpdateLotByIdAsync(id, request);
            if (lot == null)
                return NotFound();

            return Ok(lot);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteLot(Guid id)
        {
            var result = await _lotService.DeleteLotAsync(id);
            if (!result)
                return BadRequest(result);

            return NoContent();
        }

    }
}
