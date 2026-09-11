using System.ComponentModel.DataAnnotations;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.Application.ProfitLight;

public sealed record ProfitLightPreTradeRequest
{
    [Required] public string EaId { get; init; } = string.Empty;
    public long MagicNumber { get; init; }
    [Range(1, int.MaxValue)] public int AccountId { get; init; }
    [Required] public string Symbol { get; init; } = string.Empty;
    [Required] public string Timeframe { get; init; } = string.Empty;
    [Range(0, 100)] public int RawScore { get; init; }
    public bool HasSufficientEvidence { get; init; } = true;
    public bool IsDataComplete { get; init; } = true;
    public DateTime DataTimestampUtc { get; init; }
    public decimal ExpectedValueAfterCosts { get; init; }
    public ProfitLightSafetyDecision SafetyDecision { get; init; }
    public string? SafetyReason { get; init; }
}

public sealed record ProfitLightResult(
    int Id, string EaId, long MagicNumber, int AccountId, string Symbol,
    string Timeframe, int? Score, ProfitLightState LightState,
    ProfitLightAction RecommendedAction, ProfitLightConfidence Confidence,
    ProfitLightSafetyDecision SafetyDecision, decimal ExpectedValueAfterCosts,
    IReadOnlyList<string> Reasons, string DisplayLabel, string ModelVersion,
    DateTime CalculatedAtUtc, DateTime ExpiresAtUtc);
