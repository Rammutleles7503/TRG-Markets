using Microsoft.AspNetCore.Mvc;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Application.ProfitLight;

namespace TRG_Markets.API.Controllers;

[ApiController]
[Route("api/profit-light")]
public sealed class ProfitLightController(IProfitLightService service) : ControllerBase
{
    [HttpPost("pre-trade")]
    public async Task<ActionResult<ProfitLightResult>> Assess(
        [FromBody] ProfitLightPreTradeRequest request, CancellationToken cancellationToken)
        => Ok(await service.AssessPreTradeAsync(request, cancellationToken));

    [HttpGet("ea/{eaId}/latest")]
    public async Task<ActionResult<ProfitLightResult>> Latest(string eaId, CancellationToken cancellationToken)
    {
        var result = await service.GetLatestForEaAsync(eaId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}
