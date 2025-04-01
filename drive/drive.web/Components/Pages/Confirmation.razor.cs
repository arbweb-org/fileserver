using drive.web.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace drive.web.Components.Pages
{
    public partial class Confirmation : PageBase
    {
        [Parameter]
        public string Token { get; set; }

        bool isPasswordReset;

        public Confirmation(DriveDbContext dbContext)
        {
            dbx = dbContext;
        }

        void Confirm()
        {
            var invalidToken = "The confirmation link has expired or is invalid. Please request a new confirmation link to verify your email address.";

            if (Token == null)
            {
                Message(invalidToken);
                return;
            }

            var json = Helper.Decrypt(Token);
            string[] request = JsonSerializer.Deserialize<string[]>(json);

            long time = long.Parse(request[0]);
            // Token is valid for 1 hour
            if (time < DateTime.Now.AddHours(-1).ToFileTimeUtc())
            {
                Message(invalidToken);
                return;
            }

            User user = new User
            {
                Email = request[2],
                Password = request[3]
            };

            var users = (from u in dbx.Users
                         where u.Email == user.Email
                         select u).ToArray();

            isPasswordReset = request[1] == "1";

            if (isPasswordReset) { Reset(users, user); }
            else { Register(users, user); }
        }

        void Reset(User[] users, User user)
        {
            if (!users.Any())
            {
                Message("User not found");
                return;
            }

            if (users[0].Password == user.Password)
            {
                Message("New password cannot be the same as the old password");
                return;
            }

            Message("Your password has been reset. You can now login with your new password.", true);
            users[0].Password = user.Password;
            dbx.SaveChanges();
        }

        void Register(User[] users, User user) 
        {
            if (users.Any())
            {
                Message("User already exists");
                return;
            }

            dbx.Users.Add(user);
            dbx.SaveChanges();
            Message("Your account has been successfully created. You can now log in to your account with your credentials.", true);
        }

        protected override async Task Loaded()
        {
            Confirm();
            isLoaded = true;
            // Have to be called for any ui changes
            // inside OnAfterRenderAsync
            StateHasChanged();
        }
    }
}