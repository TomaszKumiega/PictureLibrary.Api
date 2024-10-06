using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.Model;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Client.Clients.Libraries
{
    internal class LibrariesClient(IApiHttpClient apiHttpClient) : ILibrariesClient
    {
        public async Task<LibraryDto> GetLibrary(string id, AuthorizationData authorizationData)
        {
            return await apiHttpClient.Get<LibraryDto>($"library/get?id={id}", authorizationData);
        }

        public async Task<LibrariesDto> GetAllLibraries(AuthorizationData authorizationData)
        {
            return await apiHttpClient.Get<LibrariesDto>("library/getall", authorizationData);
        }

        public async Task<LibraryDto> AddLibrary(NewLibraryDto request, AuthorizationData authorizationData)
        {
            return await apiHttpClient.Post<LibraryDto>("library/create", request, authorizationData);
        }

        public async Task<LibraryDto> UpdateLibrary(string libraryId, UpdateLibraryDto request, AuthorizationData authorizationData)
        {
            return await apiHttpClient.Patch<LibraryDto>($"library/update?libraryId={libraryId}", request, authorizationData);
        }

        public async Task DeleteLibrary(string id, AuthorizationData authorizationData)
        {
            await apiHttpClient.Delete($"library/delete?id={id}", authorizationData);
        }
    }
}