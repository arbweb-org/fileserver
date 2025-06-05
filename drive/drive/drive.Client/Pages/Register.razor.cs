using drive.Lib;
using drive.Lib.DTO;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace drive.Client.Pages
{
    public partial class Register : PageBase
    {
        bool isPasswordReset;
        [Parameter]
        public string IsPasswordReset
        {
            get
            {
                return isPasswordReset ? "update" : "new";
            }
            set
            {
                if (value == "new") { isPasswordReset = false; }
                else if (value == "update") { isPasswordReset = true; }
                else { throw new ArgumentException(); }
            }
        }

        string _email;
        string email
        {
            get => _email;
            set
            {
                _email = value;
                Message();
            }
        }

        string _password;
        string password
        {
            get => _password;
            set
            {
                _password = value;
                Message();
            }
        }

        string _confirm;
        string confirm
        {
            get => _confirm;
            set
            {
                _confirm = value;
                Message();
            }
        }

        bool waitingResend = true;
        bool cooldownVisible = true;
        int countdownTime;

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
            if (password != confirm)
            {
                Message("Passwords do not match");
                return false;
            }
            return true;
        }

        async Task Submit()
        {
            var request = new RegisterDTO
            {
                Email = email,
                Password = password,
                IsPasswordReset = isPasswordReset
            };
            var response = await client.PostAsJsonAsync("/api/users/register", request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var succeeded = request.IsPasswordReset ?
                    "Password reset link sent!" :
                    "Thanks for signing up!";

                Message(succeeded, true);

                // Fire and forget, run async
                MailSent();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Message(responseBody);
            }
            else
            {
                Message("Something went wrong while creating your account. Please try again.");
            }
        }

        async Task MailSent()
        {
            waitingResend = true;
            countdownTime = 60;
            cooldownVisible = true;

            for (int i = countdownTime; i > 0; i--)
            {
                countdownTime = i;
                StateHasChanged();
                await Task.Delay(1000);
            }

            waitingResend = false;
            cooldownVisible = false;
            StateHasChanged();
        }

        async Task RegisterClicked()
        {
            SetBusy(true);

            if (IsValidInputs()) { await Submit(); }

            SetBusy(false);
        }

        async Task ResendClicked()
        {
            SetBusy(true);

            await Submit();

            SetBusy(false);
        }
    }
}