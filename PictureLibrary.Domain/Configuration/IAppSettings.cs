namespace PictureLibrary.Domain.Configuration
{
    public interface IAppSettings
    {
        public string TokenPrivateKey { get; set; }
        public string DatabaseName { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
    }
}
