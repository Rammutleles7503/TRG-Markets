using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntryAuthorityController : ControllerBase
    {
        private readonly IEntryAuthorityService _entryAuthorityService;

        public EntryAuthorityController(IEntryAuthorityService entryAuthorityService)
        {
            _entryAuthorityService = entryAuthorityService;
        }

        [HttpGet("check/{tradingAccountId:int}")]
        public async Task<IActionResult> CheckEntry(int tradingAccountId)
        {
            var allowed = await _entryAuthorityService.ShouldAllowEntryAsync(tradingAccountId);
            return Ok(new { tradingAccountId, allowed });
        }
    }
}
