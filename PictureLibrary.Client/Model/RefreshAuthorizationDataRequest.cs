namespace PictureLibrary.Client.Model
{
    public class RefreshAuthorizationDataRequest
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
