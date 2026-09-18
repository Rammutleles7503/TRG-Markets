using Microsoft.EntityFrameworkCore;
using System.Linq;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Application.ProfitLight;
using TRG_Markets.Domain.Entities;
using TRG_Markets.Persistence;

namespace TRG_Markets.Infrastructure.Services;

public sealed class ProfitLightService(TRGMarketsDbContext dbContext, IEntryAuthorityService? entryAuthorityService = null) : IProfitLightService
{
    private static readonly TimeSpan MaximumDataAge = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan AssessmentLifetime = TimeSpan.FromMinutes(1);

    public async Task<ProfitLightResult> AssessPreTradeAsync(ProfitLightPreTradeRequest request, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var reasons = new List<string>();
        int? score = request.RawScore;
        var state = Band(request.RawScore);
        var action = ActionFor(state);
        var confidence = request.HasSufficientEvidence ? ProfitLightConfidence.Medium : ProfitLightConfidence.Low;

        if (!request.IsDataComplete || request.DataTimestampUtc == default || now - request.DataTimestampUtc.ToUniversalTime() > MaximumDataAge)
        {
            score = null;
            state = ProfitLightState.Black;
            action = ProfitLightAction.Unavailable;
            confidence = ProfitLightConfidence.None;
            reasons.Add("DATA_INVALID_OR_STALE");
        }
        else if (request.SafetyDecision is ProfitLightSafetyDecision.Reject or ProfitLightSafetyDecision.Emergency)
        {
            state = ProfitLightState.Black;
            action = ProfitLightAction.Reject;
            reasons.Add(request.SafetyReason ?? "SAFETY_AUTHORITY_OVERRIDE");
        }
        else if (!request.HasSufficientEvidence)
        {
            score = null;
            state = ProfitLightState.Grey;
            action = ProfitLightAction.Abstain;
            reasons.Add("INSUFFICIENT_EVIDENCE");
        }
        else if (request.ExpectedValueAfterCosts <= 0 && state is ProfitLightState.Green or ProfitLightState.BrightGreen)
        {
            state = ProfitLightState.Amber;
            action = ProfitLightAction.Wait;
            reasons.Add("COSTS_EXCEED_EDGE");
        }
        else
        {
            reasons.Add("DETERMINISTIC_V1_SCORE_BAND");
        }


        if (entryAuthorityService is not null)
        {
            bool entryAllowed = await entryAuthorityService.ShouldAllowEntryAsync(request.AccountId);
            if (!entryAllowed)
            {
                state = ProfitLightState.Black;
                action = ProfitLightAction.Reject;
                reasons.Add("ENTRY_AUTHORITY_BLOCKED");
            }
        }

        var entity = new ProfitLightAssessment
        {
            EaId = request.EaId.Trim(), MagicNumber = request.MagicNumber,
            AccountId = request.AccountId, Symbol = request.Symbol.Trim().ToUpperInvariant(),
            Timeframe = request.Timeframe.Trim().ToUpperInvariant(), Score = score,
            LightState = state, RecommendedAction = action, Confidence = confidence,
            SafetyDecision = request.SafetyDecision, ExpectedValueAfterCosts = request.ExpectedValueAfterCosts,
            Reasons = string.Join('|', reasons), CalculatedAtUtc = now,
            ExpiresAtUtc = now.Add(AssessmentLifetime)
        };

        dbContext.ProfitLightAssessments.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResult(entity);
    }

    public async Task<ProfitLightResult?> GetLatestForEaAsync(string eaId, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.ProfitLightAssessments.AsNoTracking()
            .Where(x => x.EaId == eaId)
            .OrderByDescending(x => x.CalculatedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);
        return entity is null ? null : ToResult(entity);
    }

    public static ProfitLightState Band(int score) => score switch
    {
        0 => ProfitLightState.Black,
        <= 34 => ProfitLightState.Red,
        <= 49 => ProfitLightState.Orange,
        <= 64 => ProfitLightState.Amber,
        <= 74 => ProfitLightState.Blue,
        <= 84 => ProfitLightState.Green,
        _ => ProfitLightState.BrightGreen
    };

    private static ProfitLightAction ActionFor(ProfitLightState state) => state switch
    {
        ProfitLightState.BrightGreen or ProfitLightState.Green => ProfitLightAction.Enter,
        ProfitLightState.Blue or ProfitLightState.Amber => ProfitLightAction.Wait,
        ProfitLightState.Orange => ProfitLightAction.Protect,
        ProfitLightState.Red or ProfitLightState.Black => ProfitLightAction.Reject,
        _ => ProfitLightAction.Abstain
    };

    private static ProfitLightResult ToResult(ProfitLightAssessment x) => new(
        x.Id, x.EaId, x.MagicNumber, x.AccountId, x.Symbol, x.Timeframe, x.Score,
        x.LightState, x.RecommendedAction, x.Confidence, x.SafetyDecision,
        x.ExpectedValueAfterCosts, x.Reasons.Split('|', StringSplitOptions.RemoveEmptyEntries),
        "Confidence score — probability estimate, not guaranteed", x.ModelVersion,
        x.CalculatedAtUtc, x.ExpiresAtUtc);
}
