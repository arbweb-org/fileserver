using fileserver.api.Models;
using Microsoft.AspNetCore.Mvc;

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

    private Boolean authorize()
    {
        var session = Request.Cookies["session"];
        return Helper.ValidateToken(session);
    }

    [HttpGet("get")]
    public object get()
    {
        if (!authorize()) { return "Unauthorized"; }

        return dbx.Users.ToArray();
    }

    [HttpGet("delete")]
    public object delete([FromQuery] long id)
    {
        if (!authorize()) { return "Unauthorized"; }

        dbx.Users.Remove(new User { Id = id });
        dbx.SaveChanges();

        return "Ok";
    }

    [HttpGet("add")]
    public object add([FromQuery] string email, [FromQuery] string password)
    {
        if (!authorize()) { return "Unauthorized"; }

        // encrypt email and password
        string code = Helper.Encrypt(Helper.Salt + "-" + email + "-" + password);
        string hash = Helper.Hash(code);
        string url = $"api/users/verify?code=" + code;
        Response.Redirect(url);

        return "Ok";
    }

    [HttpGet("verify")]
    public object verify([FromQuery] string code)
    {
        string[] parts = Helper.Decrypt(code).Split("-");
        if (parts.Length != 3)
        {
            return "Invalid code";
        }

        string salt = parts[0];
        string email = parts[1];
        string password = parts[2];

        if (salt != Helper.Salt)
        {
            return "Invalid code";
        }

        dbx.Users.Add(new User { Email = email, Password = Helper.Hash(password) });
        dbx.SaveChanges();
        return "Ok";
    }
}