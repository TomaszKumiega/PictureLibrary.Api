namespace PictureLibrary.Client.Requests
{
    public class UpdateUserRequest
    {
        public required string Username { get; set; }
        public string? Email { get; set; }
    }
}
