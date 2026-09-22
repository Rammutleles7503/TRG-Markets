using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Application.ProfitLight;

namespace TRG_Markets.API.Controllers
{
    [ApiController]
    [Route("api/system-orchestration")]
    public class SystemOrchestrationController : ControllerBase
    {
        private readonly ISystemOrchestrationService _systemOrchestrationService;

        public SystemOrchestrationController(ISystemOrchestrationService systemOrchestrationService)
        {
            _systemOrchestrationService = systemOrchestrationService;
        }

        [HttpGet("evaluate/{tradingAccountId:int}")]
        public async Task<IActionResult> Evaluate(int tradingAccountId)
        {
            var decision = await _systemOrchestrationService.EvaluateSystemAsync(tradingAccountId);
            return Ok(new { tradingAccountId, decision });
        }

        [HttpPost("evaluate-pretrade")]
        public async Task<IActionResult> EvaluatePreTrade([FromBody] ProfitLightPreTradeRequest request)
        {
            var decision = await _systemOrchestrationService.EvaluatePreTradeAsync(request);
            return Ok(new { request.AccountId, request.EaId, decision });
        }
    }
}
