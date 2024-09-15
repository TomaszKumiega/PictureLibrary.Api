namespace PictureLibrary.Client.Model
{
    public class AddUserRequest
    {
        public required string Username { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
    }
}
