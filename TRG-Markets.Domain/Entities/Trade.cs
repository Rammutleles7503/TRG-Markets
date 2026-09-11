using System;
using System.Collections.Generic;
using System.Text;

namespace TRG_Markets.Domain.Entities
{
    public class Trade
    {
        public int Id { get; set; }
        public int TradingAccountId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public decimal LotSize { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal Profit { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string TicketNumber { get; set; } = string.Empty;
        public long MagicNumber { get; set; }
        public int Volume { get; set; }
        public string BrokerName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string ServerName { get; set; } = string.Empty;
        public string BrokerServer { get; set; } = string.Empty;





    }
}
