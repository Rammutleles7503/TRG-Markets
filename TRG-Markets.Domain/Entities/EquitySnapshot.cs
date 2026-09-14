using System;

namespace TRG_Markets.Domain.Entities
{
    public class EquitySnapshot
    {
        public int Id { get; set; }
        public int TradingAccountId { get; set; }
        public decimal Balance { get; set; }
        public decimal Equity { get; set; }
        public decimal FloatingProfitLoss { get; set; }
        public decimal DailyProfitLoss { get; set; }
        public decimal PeakEquity { get; set; }
        public decimal DrawdownAmount { get; set; }
        public decimal DrawdownPercentage { get; set; }
        public int OpenPositions { get; set; }
        public bool TradingSuspended { get; set; }
        public string? ProtectionReason { get; set; }
        public DateTime RecordedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
