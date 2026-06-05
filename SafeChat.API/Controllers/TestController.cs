using Microsoft.AspNetCore.Mvc;

namespace SafeChat.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "SafeChat API is running!", timestamp = DateTime.UtcNow });
        }
    }
}
