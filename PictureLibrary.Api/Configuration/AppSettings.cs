using PictureLibrary.Domain.Configuration;

namespace PictureLibrary.Api.Configuration
{
    public class AppSettings : IAppSettings
    {
        public required string DatabaseName { get; set; }
        public required string TokenPrivateKey { get; set; }
        public required string JwtIssuer { get; set; }
        public required string JwtAudience { get; set; }
    }
}
