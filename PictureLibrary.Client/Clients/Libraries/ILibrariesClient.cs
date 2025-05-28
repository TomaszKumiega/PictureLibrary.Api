using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Client.Clients.Libraries;

public interface ILibrariesClient
{
    Task<LibraryDto> AddLibrary(NewLibraryDto request);
    Task DeleteLibrary(string id);
    Task<LibrariesDto> GetAllLibraries();
    Task<LibraryDto> GetLibrary(string id);
    Task<LibraryDto> UpdateLibrary(string libraryId, UpdateLibraryDto request);
}