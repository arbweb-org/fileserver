using Microsoft.AspNetCore.Mvc;

namespace fileserver.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        FilesDbContext dbx;

        public LoginController(FilesDbContext dbContext)
        {
            dbx = dbContext;
        }

        [HttpPost("admin")]
        public object log_admin(
            [FromForm] string name,
            [FromForm] string password)
        {
            if (name != "admin" || password != Helper.Password)
            {
                return "Invalid credentials";
            }

            string time = DateTime.Now.ToFileTimeUtc().ToString();
            Response.Cookies.Append("session", Helper.Encrypt(time));

            return "Ok";
        }
    }
}