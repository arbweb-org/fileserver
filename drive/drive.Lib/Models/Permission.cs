namespace drive.Lib.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public long User { get; set; }
        public long Folder { get; set; }
        // Has the permission to modify the contents
        // of the associated folder and its subfolders
        public bool Write { get; set; }
    }
}