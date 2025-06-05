using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace drive.Client
{
    public class PageBase : ComponentBase
    {
        protected enum URLs
        {
            Login,
            Drive,
            Register,
            ResetPassword      
        }

        static Dictionary<int, string> PageURLs = new Dictionary<int, string>
        {
            { (int)URLs.Login, "/" },
            { (int)URLs.Drive, "drive" },
            { (int)URLs.Register, "user/new" },
            { (int)URLs.ResetPassword, "user/update" }
        };

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        //protected DriveDbContext dbx;

        protected HttpClient client { get; set; } = new HttpClient { BaseAddress = Program.BaseAddress };

        protected bool fail;
        protected bool success;
        protected string message = string.Empty;

        // Used to show page contents, only
        // after it has been fully loaded
        // to avoid inconsistent state
        protected bool isLoaded { get; set; } = false;
        protected bool isBusy { get; set; } = true;

        //protected User currentUser = new User { Id = -1 };
        protected bool isAdmin = false;

        protected void Message(string text = "", bool ok = false)
        {
            if (string.IsNullOrEmpty(text))
            {
                message = string.Empty;
                fail = false;
                success = false;
                return;
            }

            success = ok;
            fail = !ok;
            message = text;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) { return; }

            await Load();
            isLoaded = true;
            SetBusy(false);
            StateHasChanged();
        }

        protected void SetBusy(bool busy)
        {
            isBusy = busy;
        }

        protected virtual async Task Load() { }

        protected string VisibleIf(bool condition)
        {
            return condition ? "display: block" : "display: none";
        }

        class Token
        {
            public long Id { get; set; }
            public DateTime Time { get; set; }

            public Token(long id)
            {
                Id = id;
                Time = DateTime.Now;
            }

            public string ToJson()
            {
                return JsonSerializer.Serialize(new long[] { Id, Time.ToFileTimeUtc() });
            }

            public static Token FromJson(string json)
            {
                long[]? data = JsonSerializer.Deserialize<long[]>(json);
                if (data == null || data.Length != 2)
                {
                    return new Token(-1);
                }

                return new Token(data[0])
                {
                    Time = DateTime.FromFileTimeUtc(data[1])
                };
            }
        }

        // Save encrypted user id to cookies
        protected async Task SaveToken(long id, bool persistant)
        {
            Token token = new Token(id);

            string json = token.ToJson();
            //string cipher = Helper.Encrypt(json);

            //await JSRuntime.InvokeVoidAsync("localStorageFunctions.setItem", "token", cipher);
        }

        // Get user id from cookies and load user from database
        protected async Task<bool> LoadToken()
        {
            return true;


            //// Check if user is logged in
            //string cipher = await JSRuntime.InvokeAsync<string>("localStorageFunctions.getItem", "token");
            //string json = Helper.Decrypt(cipher);
            //if (json == null)
            //{
            //    return await SignOut();
            //}

            //Token token;
            //try
            //{
            //    token = Token.FromJson(json);
            //}
            //catch
            //{
            //    // Token may be tampered with
            //    return await SignOut();
            //}

            //// Token expired after 1 day
            //if (token.Time.ToFileTimeUtc() < DateTime.Now.AddDays(-1).ToFileTimeUtc())
            //{
            //    return await SignOut();
            //}

            //User[] users = dbx.Users.Where(u => u.Id == token.Id).ToArray();
            //// User may be deleted
            //if (!users.Any())
            //{
            //    return await SignOut();
            //}

            //currentUser = users.First();
            //isAdmin = currentUser.Email == Helper.Admin;
            //return true;
        }

        async Task<bool> SignOut(bool redirect = false)
        {
            await JSRuntime.InvokeVoidAsync("SessionExpired");
            return true;
        }

        protected void Navigate(URLs url)
        {
            NavigationManager.NavigateTo(PageURLs[(int)url], true);
        }
    }
}