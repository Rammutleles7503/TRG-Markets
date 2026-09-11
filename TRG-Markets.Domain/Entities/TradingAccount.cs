using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace TRG_Markets.Domain.Entities
{
    public class TradingAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string Broker { get; set; }
        = string.Empty;
        public decimal Balance { get; set; }
        public decimal Equity { get; set; }
        public decimal FreeMargin { get; set; }
        public decimal Margin { get; set; }
        public decimal Drawdown { get; set; }
        public string Platform { get; set; } = "MT5";
        public string ServerName { get; set; } = string.Empty;
        public string AccountOwner { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string Currency { get; set; } = "USD";
        public DateTime CreatedAt { get; set; } = DateTime. UtcNow;

    }
}
