using System;

namespace fileserver.api.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public long User { get; set; }
        public long Folder { get; set; }
        // False = Read, True = Write
        public Boolean Write { get; set; }
    }
}