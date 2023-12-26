using PictureLibrary.Domain.Configuration;

namespace PictureLibrary.Api.Configuration
{
    public class AppSettings : IAppSettings
    {
        public string DatabaseName { get; set; }
    }
}
