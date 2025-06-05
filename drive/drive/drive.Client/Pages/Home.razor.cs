using drive.Lib.Models;
using File = drive.Lib.Models.File;

namespace drive.Client.Pages
{
    public partial class Home : PageBase
    {
        List<File> files = new List<File>();
        List<Folder> folders = new List<Folder>();

        protected override async Task OnInitializedAsync()
        {
            files = files = new List<File>
            {
                new File { Name = "File1.txt" },
                new File { Name = "File2.png" },
                new File { Name = "File1.ppt" },
                new File { Name = "File2.xls" },
                new File { Name = "File1.doc" },
                new File { Name = "File2.pdf" },
                new File { Name = "File3.dat" }
            };

            folders = new List<Folder>
            {
                new Folder { Name = "Folder1" },
                new Folder { Name = "Folder2" },
                new Folder { Name = "Folder3" }
            };
        }

        string FileStyle(string fileName)
        {
            string extension = fileName.Split(".").Last();
            switch (extension)
            {
                case "jpg":
                case "jpeg":
                case "png":
                case "gif":
                    return "bi-file-earmark-image-fill image-icon";
                case "pdf":
                    return "bi-file-earmark-pdf-fill pdf-icon";
                case "doc":
                case "docx":
                    return "bi-file-earmark-word-fill document-icon";
                case "ppt":
                case "pptx":
                    return "bi-file-earmark-ppt-fill presentation-icon";
                case "xls":
                case "xlsx":
                    return "bi-file-earmark-excel-fill spreadsheet-icon";
                default:
                    return "bi-file-earmark-fill file-icon";
            }
        }
    }
}