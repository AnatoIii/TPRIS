namespace AudioServer.Models.DTOs
{
    public class RegisterTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}