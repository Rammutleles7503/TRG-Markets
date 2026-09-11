using Microsoft.AspNetCore.Mvc;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradesController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTrades()
        {
            var trades = await _tradeService.GetAllTradesAsync();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTradeById(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);
            if (trade == null)
                return NotFound();
            return Ok(trade);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTrade(Trade trade)
        {
            var createdTrade = await _tradeService.CreateTradeAsync(trade);
            return CreatedAtAction(nameof(GetTradeById), new { id = createdTrade.Id }, createdTrade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrade(int id, Trade trade)
        {
            var updatedTrade = await _tradeService.UpdateTradeAsync(id, trade);
            if (updatedTrade == null)
            {
                return NotFound();
            }
            return Ok(updatedTrade);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            var deleted = await _tradeService.DeleteTradeAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
