using System;
using System.Collections.Generic;
using System.Text;

namespace TRG_Markets.Domain.Entities
{
    public class Broker
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;
        public string BrokerType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    }
}
