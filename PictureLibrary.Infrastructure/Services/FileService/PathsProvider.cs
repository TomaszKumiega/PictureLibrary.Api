using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services;

public class PathsProvider(IAppSettings appSettings) : IPathsProvider
{
    public string GetStoragePath()
    {
        return appSettings.VolumePath;
    }

    public string GetTempDirectoryPath()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}