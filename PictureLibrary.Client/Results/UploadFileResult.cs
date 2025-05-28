using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Results;

internal record FileUploadResult(bool UploadFinished, FileContentAcceptedResult? FileContentAcceptedResult, FileCreatedResult? FileCreatedResult);