namespace PictureLibrary.Domain.Configuration;

public interface IAppSettings
{
    public string TokenPrivateKey { get; }
    public string DatabaseName { get; }
    public string JwtIssuer { get; }
    public string JwtAudience { get; }
    public string VolumePath { get; }
}