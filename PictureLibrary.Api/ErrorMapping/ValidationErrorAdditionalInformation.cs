namespace PictureLibrary.Api.ErrorMapping
{
    public class ValidationErrorAdditionalInformation
    {
        public required string PropertyName { get; init; }
        public required string ErrorMessage { get; init; }
        public required string ErrorCode { get; init; }
    }
}
