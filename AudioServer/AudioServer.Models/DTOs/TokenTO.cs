namespace AudioServer.Models.DTOs
{
    public class TokenTO
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

        public User User { get; set; }
    }
}