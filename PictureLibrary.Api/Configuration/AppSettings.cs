using PictureLibrary.Domain.Configuration;

namespace PictureLibrary.Api.Configuration;

public class AppSettings(IConfiguration configuration) : IAppSettings
{
    public string DatabaseName => configuration["DatabaseName"] ?? string.Empty;
    public string TokenPrivateKey => configuration["TokenPrivateKey"] ?? string.Empty;
    public string JwtIssuer => configuration["JwtIssuer"] ?? string.Empty;
    public string JwtAudience => configuration["JwtAudience"] ?? string.Empty;
    public string VolumePath => configuration["VolumePath"] ?? string.Empty;
}