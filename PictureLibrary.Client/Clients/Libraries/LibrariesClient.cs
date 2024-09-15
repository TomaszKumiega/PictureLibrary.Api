using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Libraries
{
    public class LibrariesClient(IApiHttpClient apiHttpClient)
    {
        public async Task<Library> GetLibrary(string id)
        {
            return await apiHttpClient.Get<Library>($"library/get/{id}");
        }

        public async Task<AllLibraries> GetAllLibraries()
        {
            return await apiHttpClient.Get<AllLibraries>("library/getall");
        }

        public async Task<Library> AddLibrary(AddLibraryRequest request, AuthorizationData authorizationData)
        {
            return await apiHttpClient.Post<Library>("library/create", request, authorizationData);
        }
    }
}