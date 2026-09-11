using System;
using System.Threading.Tasks;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.Application
{
    public interface IMarketHolidayService
    {
        Task<MarketHoliday?> IsMarketHolidayAsync(DateTime date);
        Task<MarketHoliday?> GetMarketHolidayAsync(DateTime date);
        Task<MarketHoliday> CreateMarketHolidayAsync(MarketHoliday holiday);
    }
}
