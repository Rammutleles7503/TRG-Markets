using Microsoft.EntityFrameworkCore;
using Xunit;
using TRG_Markets.Application.ProfitLight;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Infrastructure.Services;
using TRG_Markets.Persistence;

namespace TRG_Markets.Tests;

public sealed class ProfitLightServiceTests
{
    public static TheoryData<int, ProfitLightState> Boundaries => new()
    {
        { 0, ProfitLightState.Black }, { 1, ProfitLightState.Red },
        { 34, ProfitLightState.Red }, { 35, ProfitLightState.Orange },
        { 49, ProfitLightState.Orange }, { 50, ProfitLightState.Amber },
        { 64, ProfitLightState.Amber }, { 65, ProfitLightState.Blue },
        { 74, ProfitLightState.Blue }, { 75, ProfitLightState.Green },
        { 84, ProfitLightState.Green }, { 85, ProfitLightState.BrightGreen },
        { 100, ProfitLightState.BrightGreen }
    };

    [Theory]
    [MemberData(nameof(Boundaries))]
    public void Score_boundaries_are_deterministic(int score, ProfitLightState expected)
        => Assert.Equal(expected, ProfitLightService.Band(score));

    [Fact]
    public async Task Safety_reject_overrides_favourable_score()
    {
        var service = CreateService();
        var result = await service.AssessPreTradeAsync(ValidRequest() with
        {
            RawScore = 95,
            SafetyDecision = ProfitLightSafetyDecision.Reject,
            SafetyReason = "ENTRY_AUTHORITY_REJECT"
        });

        Assert.Equal(ProfitLightState.Black, result.LightState);
        Assert.Equal(ProfitLightAction.Reject, result.RecommendedAction);
        Assert.Contains("ENTRY_AUTHORITY_REJECT", result.Reasons);
    }

    [Fact]
    public async Task Insufficient_evidence_abstains_with_grey()
    {
        var result = await CreateService().AssessPreTradeAsync(ValidRequest() with { HasSufficientEvidence = false });
        Assert.Null(result.Score);
        Assert.Equal(ProfitLightState.Grey, result.LightState);
        Assert.Equal(ProfitLightAction.Abstain, result.RecommendedAction);
    }

    [Fact]
    public async Task Stale_data_fails_closed()
    {
        var result = await CreateService().AssessPreTradeAsync(ValidRequest() with { DataTimestampUtc = DateTime.UtcNow.AddMinutes(-3) });
        Assert.Null(result.Score);
        Assert.Equal(ProfitLightState.Black, result.LightState);
        Assert.Equal(ProfitLightAction.Unavailable, result.RecommendedAction);
    }

    [Fact]
    public async Task Non_positive_expected_value_prevents_green()
    {
        var result = await CreateService().AssessPreTradeAsync(ValidRequest() with { RawScore = 90, ExpectedValueAfterCosts = 0 });
        Assert.Equal(ProfitLightState.Amber, result.LightState);
        Assert.Contains("COSTS_EXCEED_EDGE", result.Reasons);
    }

    private static ProfitLightService CreateService()
    {
        var options = new DbContextOptionsBuilder<TRGMarketsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new ProfitLightService(new TRGMarketsDbContext(options));
    }

    private static ProfitLightPreTradeRequest ValidRequest() => new()
    {
        EaId = "Gold-Scalper-PRO", MagicNumber = 710002, AccountId = 1,
        Symbol = "XAUUSD", Timeframe = "M15", RawScore = 80,
        DataTimestampUtc = DateTime.UtcNow, ExpectedValueAfterCosts = 12.50m,
        SafetyDecision = ProfitLightSafetyDecision.Allow
    };
}
