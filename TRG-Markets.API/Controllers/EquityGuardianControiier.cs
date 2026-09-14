using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.API.Controllers
{
    [ApiController]
    [Route("api/equity-guardian")]
    public class EquityGuardianController : ControllerBase
    {
        private readonly IEquityGuardianService _service;

        public EquityGuardianController(IEquityGuardianService service)
        {
            _service = service;
        }

        [HttpPost("snapshot")]
        public async Task<ActionResult<EquitySnapshot>> RecordSnapshot([FromBody] EquitySnapshot snapshot)
        {
            if (snapshot == null)
                return BadRequest("Snapshot is required.");

            if (snapshot.TradingAccountId <= 0)
                return BadRequest("A valid trading account ID is required.");

            if (snapshot.Balance < 0 || snapshot.Equity < 0)
                return BadRequest("Balance and Equity cannot be negative.");

            var result = await _service.RecordSnapshotAsync(snapshot);
            return CreatedAtAction(nameof(GetLatest), new { tradingAccountId = result.TradingAccountId }, result);
        }

        [HttpGet("{tradingAccountId:int}/latest")]
        public async Task<ActionResult<EquitySnapshot>> GetLatest(int tradingAccountId)
        {
            var snapshot = await _service.GetLatestSnapshotAsync(tradingAccountId);
            if (snapshot == null)
                return NotFound();
            return Ok(snapshot);
        }

        [HttpGet("{tradingAccountId:int}/history")]
        public async Task<ActionResult<IReadOnlyList<EquitySnapshot>>> GetHistory(int tradingAccountId, [FromQuery] DateTime fromUtc, [FromQuery] DateTime toUtc)
        {
            if (fromUtc > toUtc)
                return BadRequest("The starting date must be before the ending date.");

            var history = await _service.GetHistoryAsync(tradingAccountId, fromUtc, toUtc);
            return Ok(history);
        }

        [HttpGet("{tradingAccountId:int}/suspension-status")]
        public async Task<IActionResult> GetSuspensionStatus(int tradingAccountId)
        {
            var suspended = await _service.ShouldSuspendTradingAsync(tradingAccountId);
            return Ok(new { tradingAccountId, tradingSuspended = suspended });
        }
    }
}
