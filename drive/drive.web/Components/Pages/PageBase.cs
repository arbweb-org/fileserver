using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace drive.web.Components.Pages
{
    public class PageBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected DriveDbContext dbx;

        bool isBusy = true;

        protected bool fail;
        protected bool success;
        protected string message = string.Empty;

        // Show page content only after it has been loaded
        protected bool isLoaded { get; set; } = false;

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
            await base.OnAfterRenderAsync(firstRender);
            if (!firstRender) { return; }

            await Loaded();

            await SetBusy(false);
        }

        protected virtual async Task Loaded() { }

        protected async Task<bool> SetBusy(bool setBusy)
        {
            if (isBusy == setBusy) { return false; }

            isBusy = setBusy;
            await JSRuntime.InvokeVoidAsync("SetBusy", isBusy);
            return true;
        }

        protected string VisibleIf(bool condition)
        {
            return condition ? "display: block" : "display: none";
        }
    }
}