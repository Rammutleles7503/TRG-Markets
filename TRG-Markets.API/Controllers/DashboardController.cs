using Microsoft.AspNetCore.Mvc;
namespace TRG_Markets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase

    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Platform = "TRG-Markets",
                Status = "Online",
                TotalUsers = 0,
                ActiveSubscriptions = 0,
                ConnectedBroker = 5,
                RunningEAs = 5
            });
        } //
    } //
} //
