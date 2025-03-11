using fileserver.api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace fileserver.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    FilesDbContext dbx;

    public UsersController(FilesDbContext dbContext)
    {
        dbx = dbContext;
    }

    long? _id = null;
    private long id
    {
        get
        {
            if (_id != null) { return (long)_id; }

            var session = Request.Cookies["session"];
            _id = Helper.ValidateToken(session);
            return (long)_id;
        }
    }

    [HttpGet("get")]
    public object get()
    {
        if (id != 0) { return "Unauthorized"; }

        return dbx.Users.ToArray();
    }

    [HttpGet("delete")]
    public object delete([FromQuery] long id)
    {
        if (id != 0) { return "Unauthorized"; }

        dbx.Users.Remove(new User { Id = id });
        dbx.SaveChanges();

        return "Ok";
    }

    // No Auth
    [HttpPost("login")]
    public object login(
        [FromForm] string user,
        [FromForm] string password)
    {
        long id = 0;
        if (user == "admin")
        {
            if (password != Helper.Password)
            {
                return "Invalid credentials";
            }
        }
        else
        {
            User[] users = (from i in dbx.Users
                            where i.Email == user &&
                            i.Password == Helper.Hash(password)
                            select i).ToArray();

            if (users.Any())
            {
                id = users[0].Id;
            }
            else
            {
                return "Invalid credentials";
            }
        }

        string time = DateTime.Now.ToFileTimeUtc().ToString();
        Response.Cookies.Append("session", Helper.Encrypt(time + "-" + id));

        return "Ok";
    }

    // No Auth
    [HttpPost("register")]
    public object register( 
        [FromForm] string email, 
        [FromForm] string password)
    {
        // Check if organization is valid
        if (!email.EndsWith("@" + Helper.Organization))
        {
            // return "Invalid email";
        }

        // Encrypt email and password
        string code = Helper.Encrypt(email + "-" + password);
        string url = $"https://drive.arbweb.org/api/users/verify?code=" + code;

        // Send verification email
        Helper.Email(email, "Verify your account", "Please click the link to verify your email: " + url);
        return "Ok";
    }

    // No Auth
    [HttpGet("verify")]
    public object verify([FromQuery] string code)
    {
        string user = Helper.Decrypt(code);
        if (user == null)
        {
            return "Invalid code";
        }

        string[] parts = user.Split("-");
        string email = parts[0];
        string password = parts[1];

        dbx.Users.Add(new User { Email = email, Password = Helper.Hash(password) });
        dbx.SaveChanges();

        return "Email verified successfully";
    }
}