using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Domain.Entities;

namespace TRG_Markets.API.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemAlertsController : ControllerBase
    {
        private readonly ISystemAlertService _systemAlertService;
        public SystemAlertsController(ISystemAlertService systemAlertService)
        {
            _systemAlertService = systemAlertService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSystemAlerts()
        {
            var alerts = await _systemAlertService.GetAllAlertsAsync();
            return Ok(alerts);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SystemAlert), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSystemAlert(int id)
        {
            var systemAlert = await _systemAlertService.GetAlertAsync(id);
            if (systemAlert == null)
            {
                return NotFound();
            }
            return Ok(systemAlert);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SystemAlert), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSystemAlert([FromBody] SystemAlert alert)
        {
            var created = await _systemAlertService.CreateAlertAsync(alert);
            return CreatedAtAction(nameof(GetSystemAlert), new { id = created.Id }, created);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SystemAlert), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSystemAlert(int id, [FromBody] SystemAlert alert)
        {
            var updated = await _systemAlertService.UpdateAlertAsync(id, alert);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSystemAlert(int id)
        {
            var deleted = await _systemAlertService.DeleteAlertAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
