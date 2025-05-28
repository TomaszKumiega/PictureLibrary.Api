namespace PictureLibrary.Application.Query;

public record GetImageFileContentResult(Stream Content, string ContentType);