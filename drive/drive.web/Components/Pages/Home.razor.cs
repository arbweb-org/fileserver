using System.Collections.Generic;
using System.Threading.Tasks;

namespace drive.web.Components.Pages
{
    public partial class Home
    {
        List<string> folders = new();
        List<string> files = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        string fileStyle(string file)
        {
            if (file.EndsWith(".jpg") || file.EndsWith(".jpeg") || file.EndsWith(".png") || file.EndsWith(".gif"))
            {
                return "bi-file-earmark-image-fill image-icon";
            }
            if (file.EndsWith(".pdf"))
            {
                return "bi-file-earmark-pdf-fill pdf-icon";
            }
            if (file.EndsWith(".doc") || file.EndsWith(".docx"))
            {
                return "bi-file-earmark-word-fill document-icon";
            }
            if (file.EndsWith(".ppt") || file.EndsWith(".pptx"))
            {
                return "bi-file-earmark-ppt-fill presentation-icon";
            }
            if (file.EndsWith(".xls") || file.EndsWith(".xlsx"))
            {
                return "bi-file-earmark-excel-fill spreadsheet-icon";
            }
            return "bi-file-earmark-fill file-icon";
        }
    }
}