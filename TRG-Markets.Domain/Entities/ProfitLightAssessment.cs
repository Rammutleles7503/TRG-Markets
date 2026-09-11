namespace TRG_Markets.Domain.Entities;

public class ProfitLightAssessment
{
    public int Id { get; set; }
    public string EaId { get; set; } = string.Empty;
    public long MagicNumber { get; set; }
    public int AccountId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Timeframe { get; set; } = string.Empty;
    public int? Score { get; set; }
    public ProfitLightState LightState { get; set; }
    public ProfitLightAction RecommendedAction { get; set; }
    public ProfitLightConfidence Confidence { get; set; }
    public ProfitLightSafetyDecision SafetyDecision { get; set; }
    public decimal ExpectedValueAfterCosts { get; set; }
    public string Reasons { get; set; } = string.Empty;
    public string ModelVersion { get; set; } = "profit-light-v1";
    public DateTime CalculatedAtUtc { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
}
