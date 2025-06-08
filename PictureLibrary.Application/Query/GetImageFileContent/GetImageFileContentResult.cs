namespace PictureLibrary.Application.Query.GetImageFileContent;

public record GetImageFileContentResult(Stream Content, string ContentType);