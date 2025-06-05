using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace drive.Client.Pages
{
    public partial class Confirmation : PageBase
    {
        [Parameter]
        public string Token { get; set; }

        protected override async Task Load()
        {
            if (Token == null)
            {
                Message("The confirmation link has expired or is invalid. Please request a new confirmation link to verify your email address.");
                return;
            }

            // Send token to api
            var response = await client.PostAsJsonAsync("/api/users/confirm", Token);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Message(responseBody, true);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Message(responseBody);
            }
            else
            {
                Message("Something went wrong while confirming your email. Please try again.");
            }
        }
    }
}