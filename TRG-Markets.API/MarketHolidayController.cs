using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TRG_Markets.Application;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketHolidayController : ControllerBase
    {
        private readonly IMarketHolidayService _marketHolidayService;

        public MarketHolidayController(IMarketHolidayService marketHolidayService)
        {
            ArgumentNullException.ThrowIfNull(marketHolidayService);
            _marketHolidayService = marketHolidayService;
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckHoliday(DateTime date)
        {
            var holiday = await _marketHolidayService.IsMarketHolidayAsync(date);
            var isHoliday = holiday != null;
            return Ok(isHoliday);
        }

        [HttpGet("holiday")]
        public async Task<IActionResult> GetHoliday(DateTime date)
        {
            var holiday = await _marketHolidayService.GetMarketHolidayAsync(date);
            if (holiday == null) return NotFound(0);
            return Ok(holiday);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHoliday(MarketHoliday holiday)
        {
            if (holiday == null) return BadRequest();

            // Persist the holiday using the service before returning Created response
            holiday = await _marketHolidayService.CreateMarketHolidayAsync(holiday);

            return CreatedAtAction(nameof(GetHoliday), new { date = holiday.HolidayDate }, holiday);
        }
    }
}
