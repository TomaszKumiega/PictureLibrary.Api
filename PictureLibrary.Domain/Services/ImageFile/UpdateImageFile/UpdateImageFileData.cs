namespace PictureLibrary.Domain.Services;

public class UpdateImageFileData
{
    public string? FileName { get; set; }
    public string? LibraryId { get; set; }
    public IEnumerable<string>? TagIds { get; set; }
}