namespace PictureLibrary.Contracts
{
    public class RefreshAuthorizationDataDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
