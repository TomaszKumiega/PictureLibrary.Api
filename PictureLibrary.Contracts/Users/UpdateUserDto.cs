namespace PictureLibrary.Contracts;

public class UpdateUserDto
{
    public required string Username { get; set; }
    public string? Email { get; set; }
}