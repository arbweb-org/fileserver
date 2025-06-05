using Microsoft.AspNetCore.Mvc;

namespace drive.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErrorLogController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetErrorLog()
        {
            // Return the static errorLog field from the Program class
            return Ok(Program.errorLog);
        }
    }
}