using PictureLibrary.Domain.Configuration;

namespace PictureLibrary.Api.Configuration
{
    public class AppSettings : IAppSettings
    {
        public AppSettings(IConfiguration configuration)
        {
            DatabaseName = configuration["DatabaseName"] ?? string.Empty;
            TokenPrivateKey = configuration["TokenPrivateKey"] ?? string.Empty;
            JwtIssuer = configuration["JwtIssuer"] ?? string.Empty;
            JwtAudience = configuration["JwtAudience"] ?? string.Empty;
            VolumePath = configuration["VolumePath"] ?? string.Empty;
        }

        public required string DatabaseName { get; set; }
        public required string TokenPrivateKey { get; set; }
        public required string JwtIssuer { get; set; }
        public required string JwtAudience { get; set; }
        public string VolumePath { get; set; }
    }
}
