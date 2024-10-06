using PictureLibrary.Client.Model;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Client.Clients.Libraries
{
    public interface ILibrariesClient
    {
        Task<LibraryDto> AddLibrary(NewLibraryDto request, AuthorizationData authorizationData);
        Task DeleteLibrary(string id, AuthorizationData authorizationData);
        Task<LibrariesDto> GetAllLibraries(AuthorizationData authorizationData);
        Task<LibraryDto> GetLibrary(string id, AuthorizationData authorizationData);
        Task<LibraryDto> UpdateLibrary(string libraryId, UpdateLibraryDto request, AuthorizationData authorizationData);
    }
}