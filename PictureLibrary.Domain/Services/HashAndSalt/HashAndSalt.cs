namespace PictureLibrary.Domain.Services;

public class HashAndSalt
{
    public required string Text { get; set; }
    public required byte[] Hash { get; set; }
    public required byte[] Salt { get; set; }
}