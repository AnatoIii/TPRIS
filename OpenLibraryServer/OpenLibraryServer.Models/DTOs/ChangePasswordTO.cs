namespace OpenLibraryServer.Models.DTOs
{
    public class ChangePasswordTO
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}