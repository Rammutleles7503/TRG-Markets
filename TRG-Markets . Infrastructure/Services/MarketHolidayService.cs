using Microsoft.EntityFrameworkCore;
using TRG_Markets.Persistence;
using TRG_Markets.Application;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRG_Markets.Domain.Entities;
namespace TRG_Markets.Infrastructure.Services
{
    public class MarketHolidayService : IMarketHolidayService
    {
        private readonly TRGMarketsDbContext _context;
        public MarketHolidayService(TRGMarketsDbContext context)
        {
            _context = context;
        }
        public async Task<MarketHoliday?> IsMarketHolidayAsync(DateTime date)
        {
            // Return the matching holiday (if any) for the specified date
            return await _context.MarketHolidays.FirstOrDefaultAsync(h => h.HolidayDate.Date == date.Date);
        }

        public async Task<MarketHoliday?> GetMarketHolidayAsync(DateTime date)
        {
            // Use EF Core async FirstOrDefault to return the matching holiday (if any)
            return await _context.MarketHolidays.FirstOrDefaultAsync(h => h.HolidayDate.Date == date.Date);
        }

        public async Task<MarketHoliday> CreateMarketHolidayAsync(MarketHoliday holiday)
        {
            if (holiday == null) throw new ArgumentNullException(nameof(holiday));

            _ = await _context.MarketHolidays.AddAsync(holiday);
            _ = await _context.SaveChangesAsync();

            return holiday;
        }
    }
}
