using drive.web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace drive.web.Components.Pages
{
    public partial class Home : PageBase
    {
        List<Folder> folders = new();
        List<File> files = new();

        public Home(DriveDbContext dbContext)
        {
            dbx = dbContext;
        }

        string FileStyle(string file)
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

        protected override async Task Loaded()
        {
            folders = await dbx.Folders.ToListAsync();
            files = await dbx.Files.ToListAsync();
            isLoaded = true;
            StateHasChanged();
        }
    }
}