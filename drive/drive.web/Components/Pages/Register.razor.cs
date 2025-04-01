using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace drive.web.Components.Pages
{
    public partial class Register : PageBase
    {
        bool isPasswordReset;
        [Parameter]
        public string IsPasswordReset
        {
            get
            {
                return isPasswordReset ? "password" : "register";
            }
            set
            {
                if (value == "register") { isPasswordReset = false; }
                else if (value == "password") { isPasswordReset = true; }
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

        string verificationUrl = string.Empty;

        bool IsValidInputs()
        {
            if (!Helper.IsValidEmail(email))
            {
                Message("Please enter a valid email address");
                return false;
            }
            if (!Helper.IsValidPassword(password))
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

        void SendMail()
        {
            // Send verification email
            Helper.Email(email, "Verify your account", "Please click the link to verify your email: " + verificationUrl);
        }

        async Task Submit()
        {
            string[] request =
                {
                    DateTime.Now.ToFileTimeUtc().ToString(),
                    isPasswordReset ? "1" : "0",
                    email,
                    password
                };

            string json = JsonSerializer.Serialize(request);
            string token = Helper.Encrypt(json);
            verificationUrl = $"https://drive.arbweb.org/user/verify/" + token;

            SendMail();
            string text = isPasswordReset ?
                "Password update request sent." :
                "Account registration request sent.";

            Message(text, true);
            await JSRuntime.InvokeVoidAsync("MailSent");
        }

        async Task RegisterClicked()
        {
            if (!await SetBusy(true)) { return; }

            if (IsValidInputs()) { await Submit(); }

            await SetBusy(false);
        }

        async Task ResendClicked()
        {
            if (!await SetBusy(true)) { return; }

            SendMail();
            await JSRuntime.InvokeVoidAsync("MailSent");

            await SetBusy(false);
        }
    }
}