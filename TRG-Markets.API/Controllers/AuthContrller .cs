using Microsoft.AspNetCore.Mvc;
namespace TRG_Markets.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase

    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Authenticated");
        }
    }
}