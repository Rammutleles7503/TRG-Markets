using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.API.Controllers
{
    [ApiController]
    [Route("api/mt5-bridge")]
    public sealed class Mt5BridgeController : ControllerBase
    {
        private readonly IMt5BridgeService _mt5BridgeService;

        public Mt5BridgeController(IMt5BridgeService mt5BridgeService)
        {
            _mt5BridgeService = mt5BridgeService;
        }

        [HttpGet("status")]
        public async Task<ActionResult<Mt5BridgeStatus>> GetStatus(CancellationToken cancellationToken = default)
        {
            var status = await _mt5BridgeService.GetStatusAsync(cancellationToken);
            return Ok(status);
        }

        [HttpPost("commands")]
        public async Task<ActionResult<Mt5CommandResult>> SendCommand([FromBody] TRG_Markets.Application.Interfaces.Mt5TradeCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mt5BridgeService.SendCommandAsync(command, cancellationToken);
            return result.Accepted ? Ok(result) : BadRequest(result);
        }
    }
}
