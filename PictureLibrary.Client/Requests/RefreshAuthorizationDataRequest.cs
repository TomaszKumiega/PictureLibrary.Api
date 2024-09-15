namespace PictureLibrary.Client.Requests
{
    public class RefreshAuthorizationDataRequest
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
