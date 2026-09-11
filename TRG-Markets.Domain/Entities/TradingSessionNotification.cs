using System;
using System.Collections.Generic;
using System.Text;

namespace TRG_Markets.Domain.Entities
{
    public class TradingSessionNotification
    {
        public int Id { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public string EvenType { get; set; } = string.Empty;
        public DateTime NotificationTime { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
