using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TRG_Markets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Users API is working");
        }
    }
}
