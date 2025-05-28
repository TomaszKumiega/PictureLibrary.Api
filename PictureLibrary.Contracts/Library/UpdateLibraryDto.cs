namespace PictureLibrary.Contracts;

public class UpdateLibraryDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
}