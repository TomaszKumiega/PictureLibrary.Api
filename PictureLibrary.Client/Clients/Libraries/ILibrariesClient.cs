using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Libraries
{
    public interface ILibrariesClient
    {
        Task<Library> AddLibrary(AddLibraryRequest request, AuthorizationData authorizationData);
        Task DeleteLibrary(string id, AuthorizationData authorizationData);
        Task<AllLibraries> GetAllLibraries(AuthorizationData authorizationData);
        Task<Library> GetLibrary(string id, AuthorizationData authorizationData);
        Task<Library> UpdateLibrary(string libraryId, UpdateLibraryRequest request, AuthorizationData authorizationData);
    }
}