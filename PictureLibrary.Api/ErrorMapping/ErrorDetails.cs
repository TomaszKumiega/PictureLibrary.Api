namespace PictureLibrary.Api.ErrorMapping;

public class ErrorDetails
{
    public required int StatusCode { get; init; }
    public required ErrorCode ErrorCode { get; init; }
    public required string Message { get; init; }
    public Dictionary<string, object>? AdditionalInformation { get; init; }
}