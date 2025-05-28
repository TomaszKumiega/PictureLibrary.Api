using PictureLibrary.Client.BaseClient;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Client.Clients.Libraries;

internal class LibrariesClient(IApiHttpClient apiHttpClient) : ILibrariesClient
{
    public async Task<LibraryDto> GetLibrary(string id)
    {
        return await apiHttpClient.Get<LibraryDto>($"library/get?id={id}");
    }

    public async Task<LibrariesDto> GetAllLibraries()
    {
        return await apiHttpClient.Get<LibrariesDto>("library/getall");
    }

    public async Task<LibraryDto> AddLibrary(NewLibraryDto request)
    {
        return await apiHttpClient.Post<LibraryDto>("library/create", request);
    }

    public async Task<LibraryDto> UpdateLibrary(string libraryId, UpdateLibraryDto request)
    {
        return await apiHttpClient.Patch<LibraryDto>($"library/update?libraryId={libraryId}", request);
    }

    public async Task DeleteLibrary(string id)
    {
        await apiHttpClient.Delete($"library/delete?id={id}");
    }
}