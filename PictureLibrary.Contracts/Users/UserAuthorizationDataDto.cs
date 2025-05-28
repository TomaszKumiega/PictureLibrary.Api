namespace PictureLibrary.Contracts;

public class UserAuthorizationDataDto
{
    public required string UserId { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime ExpiryDate { get; set; }
}