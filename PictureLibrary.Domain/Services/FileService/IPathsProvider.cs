namespace PictureLibrary.Domain.Services
{
    public interface IPathsProvider
    {
        string GetTempDirectoryPath();
        string GetStoragePath();
    }
}
