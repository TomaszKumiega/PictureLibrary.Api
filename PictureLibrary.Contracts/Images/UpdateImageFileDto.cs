namespace PictureLibrary.Contracts;

public class UpdateImageFileDto
{
    public string? FileName { get; set; }
    public string? LibraryId { get; set; }
    public IEnumerable<string>? TagIds { get; set; }
}