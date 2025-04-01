namespace drive.web.Models
{
    public class Folder
    {
        public long Id { get; set; }
        public long Parent { get; set; }
        public string Name { get; set; }
    }
}