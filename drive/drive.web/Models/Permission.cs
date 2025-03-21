namespace fileserver.api.Models
{
    public enum PermissionType
    {
        // Read the contents of the associated folder and its subfolders
        Read,
        // Modify the contents of the associated folder and its subfolders
        Write
    }

    public class Permission
    {
        public int Id { get; set; }
        public long Group { get; set; }
        public long Folder { get; set; }
        public PermissionType Type { get; set; }
    }
}