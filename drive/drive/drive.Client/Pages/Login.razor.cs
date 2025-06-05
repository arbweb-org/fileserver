using drive.Lib;
using drive.Lib.DTO;
using System.Net;
using System.Net.Http.Json;

namespace drive.Client.Pages
{
    public partial class Login : PageBase
    {

        string _email;
        string email
        {
            get => _email;
            set
            {
                _email = value;
                fail = false;
            }
        }

        string _password;
        string password
        {
            get => _password;
            set
            {
                _password = value;
                fail = false;
            }
        }

        bool rememberMe { get; set; }

        bool IsValidInputs()
        {
            if (!Core.IsValidEmail(email))
            {
                Message("Please enter a valid email address");
                return false;
            }
            if (!Core.IsValidPassword(password))
            {
                Message("Please enter a valid password");
                return false;
            }
            return true;
        }

        async Task Submit()
        {
            if (!IsValidInputs())
            {
                Message("Invalid email or password");
                return;
            }

            var request = new LoginDTO
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe
            };
            var response = await client.PostAsJsonAsync("/api/users/login", request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Navigate(URLs.Drive);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Message(responseBody);
            }
            else
            {
                Message("Something went wrong while logging in. Please try again.");
            }
        }

        private async Task LoginClicked()
        {
            SetBusy(true);

            await Submit();

            SetBusy(false);
        }
    }
}