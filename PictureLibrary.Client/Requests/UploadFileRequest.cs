using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Requests;

public record UploadFileRequest(CreateImageUploadSessionDto UploadSessionDto, Stream Content);