using TRG_Markets.Application.ProfitLight;

namespace TRG_Markets.Application.Interfaces;

public interface IProfitLightService
{
    Task<ProfitLightResult> AssessPreTradeAsync(ProfitLightPreTradeRequest request, CancellationToken cancellationToken = default);
    Task<ProfitLightResult?> GetLatestForEaAsync(string eaId, CancellationToken cancellationToken = default);
}
