namespace fileserver.api.Models
{
    public class User
    {
        public long Id { get; set; }
        public long Group { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
    }
}