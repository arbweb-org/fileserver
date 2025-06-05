namespace drive.Lib.DTO
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsPasswordReset { get; set; }
    }
}