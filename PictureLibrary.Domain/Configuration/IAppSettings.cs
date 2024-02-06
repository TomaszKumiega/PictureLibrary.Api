namespace PictureLibrary.Domain.Configuration
{
    public interface IAppSettings
    {
        public string TokenPrivateKey { get; set; }
        public string DatabaseName { get; set; }
    }
}
