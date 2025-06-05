using drive.Lib;
using drive.Lib.DTO;
using drive.Lib.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace drive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        DriveDbContext dbx;

        public UsersController(DriveDbContext db)
        {
            dbx = db;
        }

        void SendMail(string email, string verificationUrl)
        {
            Helper.Email(email, "Verify your account", "Please click the link to verify your email: " + verificationUrl);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO request)
        {
            if (!Core.IsValidEmail(request.Email))
            {
                return BadRequest("Invalid email address");
            }
            if (!Core.IsValidPassword(request.Password))
            {
                return BadRequest("Invalid password");
            }

            string[] code = {
                DateTime.Now.ToFileTimeUtc().ToString(),
                request.IsPasswordReset ? "1" : "0",
                request.Email,
                request.Password
            };

            string json = JsonSerializer.Serialize(code);
            string token = Helper.Encrypt(json);
            var verificationUrl = $"https://drive.arbweb.org/user/verify/" + token;

            SendMail(request.Email, verificationUrl);
            return Ok();
        }

        [HttpPost("confirm")]
        public IActionResult Confirm([FromBody] string token)
        {
            string[] code;
            try
            {
                var json = Helper.Decrypt(token);
                code = JsonSerializer.Deserialize<string[]>(json);
            }
            catch (Exception ex)
            {
                return BadRequest("The confirmation link has expired or is invalid. Please request a new confirmation link to verify your email address.");
            }

            DateTime timeStamp = DateTime.FromFileTimeUtc(long.Parse(code[0]));
            // Token is valid for 1 hour
            if (DateTime.UtcNow - timeStamp > TimeSpan.FromHours(1))
            {
                return BadRequest("The confirmation link has expired. Please request a new confirmation link to verify your email address.");
            }

            User user = new User
            {
                Email = code[2],
                Password = Helper.Hash(code[3])
            };

            bool isPasswordReset = code[1] == "1";
            var users = dbx.Users.Where(u => u.Email == user.Email).ToArray();
            if (isPasswordReset)
            {
                return Update(user, users);
            }
            else
            {
                return Add(user, users);
            }
        }

        IActionResult Add(User user, User[] users)
        {
            if (users.Any())
            {
                return BadRequest("An account with this email already exists. Try logging in instead.");
            }

            dbx.Users.Add(user);
            dbx.SaveChanges();

            return Ok("Your account has been successfully created. You can now log in to your account with your credentials.");
        }

        IActionResult Update(User user, User[] users)
        {
            if (!users.Any())
            {
                return BadRequest("We couldn’t update the password. This account may no longer exist or is not registered.");
            }

            if (users.First().Password == user.Password)
            {
                return BadRequest("New password cannot be the same as the old password");
            }

            users.First().Password = user.Password;
            dbx.SaveChanges();

            return Ok("Your password has been reset. You can now login with your new password.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            if (!Core.IsValidEmail(request.Email))
            {
                return BadRequest("Invalid email address");
            }

            if (request.Password.Length > 16)
            {
                return BadRequest("Invalid password");
            }

            var users = dbx.Users.Where(u => u.Email == request.Email && u.Password == Helper.Hash(request.Password)).ToArray();
            if (!users.Any())
            {
                return BadRequest("Wrong email or password");
            }

            string[] code = {
                DateTime.Now.ToFileTimeUtc().ToString(),
                users.First().Id.ToString()
            };
            string json = JsonSerializer.Serialize(code);
            string token = Helper.Encrypt(json);

            // For web clients, set the token in a cookie
            SetCookies(token, request.RememberMe);

            // For mobile clients, return the token in the response body
            return Ok(token);
        }

        void SetCookies(string token, bool rememberMe)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            if (rememberMe)
            {
                cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(7);
            }

            Response.Cookies.Append("token", token, cookieOptions);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMonths(-1),
            };
            Response.Cookies.Append("token", string.Empty, cookieOptions);
            return Ok();
        }
    }
}