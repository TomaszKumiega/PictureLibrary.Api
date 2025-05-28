namespace PictureLibrary.Contracts;

public class UserDto
{
    public required string Id { get; set; } 
    public required string Username { get; set; }
    public string? Email { get; set; }
}