using fileserver.api;
using System;
using System.Threading.Tasks;

namespace drive.web.Components.Pages
{
    public partial class Register
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
        string _confirm;
        string confirm
        {
            get => _confirm;
            set
            {
                _confirm = value;
                fail = false;
            }
        }

        string message { get; set; }

        Boolean fail { get; set; }
        Boolean success { get; set; }

        void Message(string text, Boolean ok)
        {
            success = ok;
            fail = !ok;
            message = text;
        }

        void Submit()
        {
            if (!Helper.IsValidEmail(_email))
            {
                Message("Invalid email", false);
                return;
            }
            if (!Helper.IsValidPassword(_password))
            {
                Message("Invalid password", false);
                return;
            }
            if (_password != _confirm)
            {
                Message("Passwords do not match", false);
                return;
            }

            Message("Registration successful! Please check your email.", true);
        }
    }
}