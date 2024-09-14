namespace PictureLibrary.Api.ErrorMapping
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public required string Message { get; set; }
    }
}
