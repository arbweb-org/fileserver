namespace drive.web.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public long Group { get; set; }
        public long Folder { get; set; }
        // Has the permition to modify the contents
        // of the associated folder and its subfolders
        public bool Write { get; set; }
    }
}