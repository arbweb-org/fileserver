namespace drive.web.Models
{
    public class File
    {
        public long Id { get; set; }
        public long Folder { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Description { get; set; }
    }
}