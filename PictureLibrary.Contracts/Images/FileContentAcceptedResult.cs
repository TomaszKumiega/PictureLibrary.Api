namespace PictureLibrary.Contracts;

public record FileContentAcceptedResult(string UploadSessionId, string? ExpectedRanges);