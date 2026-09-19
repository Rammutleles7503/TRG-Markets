using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.API.Controllers
{
    [ApiController]
    [Route("api/emergency-control")]
    public class EmergencyControlController : ControllerBase
    {
        private readonly IEmergencyControlService _emergencyControlService;

        public EmergencyControlController(IEmergencyControlService emergencyControlService)
        {
            _emergencyControlService = emergencyControlService;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                mode = _emergencyControlService.CurrentMode.ToString(),
                newTradesAllowed = _emergencyControlService.CurrentMode == EmergencyControlMode.Normal
            });
        }

        [HttpPost("emergency-stop")]
        public async Task<IActionResult> ActivateEmergencyStop([FromBody] EmergencyControlRequest request)
        {
            await _emergencyControlService.ActivateEmergencyStopAsync(request.Reason);
            return Ok(new
            {
                mode = _emergencyControlService.CurrentMode.ToString(),
                message = "Emergency Stop activated. New trading is blocked."
            });
        }

        [HttpPost("protect-only")]
        public async Task<IActionResult> ActivateProtectOnly([FromBody] EmergencyControlRequest request)
        {
            await _emergencyControlService.ActivateProtectOnlyModeAsync(request.Reason);
            return Ok(new
            {
                mode = _emergencyControlService.CurrentMode.ToString(),
                message = "Protect-Only mode activated."
            });
        }

        [HttpPost("resume-normal")]
        public async Task<IActionResult> ResumeNormalTrading([FromBody] EmergencyControlRequest request)
        {
            await _emergencyControlService.ResumeNormalTradingAsync(request.Reason);
            return Ok(new
            {
                mode = _emergencyControlService.CurrentMode.ToString(),
                message = "Normal trading mode restored."
            });
        }
    }

    public sealed record EmergencyControlRequest(string Reason);
}
