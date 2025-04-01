using Azure;
using System.Linq;
using System.Threading.Tasks;

namespace drive.web.Components.Pages
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

        public Login(DriveDbContext dbContext)
        {
            dbx = dbContext;
        }

        bool IsValidInputs()
        {
            if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_password))
            {
                return false;
            }

            return true;
        }

        void Submit()
        {
            var invalidInputs = "Invalid email or password";
            if (!IsValidInputs())
            {
                Message(invalidInputs);
                return;
            }

            var users = dbx.Users.Where(u => u.Email == _email && u.Password == _password);
            if (!users.Any())
            {
                Message(invalidInputs);
                return;
            }
            
            NavigationManager.NavigateTo("/home");
        }

        private async Task LoginClicked()
        {
            if (!await SetBusy(true)) { return; }

            Submit();

            await SetBusy(false);
        }
    }
}