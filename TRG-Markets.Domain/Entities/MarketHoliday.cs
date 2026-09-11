using System;
using System.Collections.Generic;
using System.Text;

namespace TRG_Markets.Domain.Entities
{
   public class MarketHoliday
    {
        public int Id { get; set; }
        public string HolidayName { get; set; } = string.Empty;
        public DateTime HolidayDate { get; set; }
        public string Country { get; set; } = string.Empty;
        public bool IsMarketClosed { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
