using PictureLibrary.Client.Model;

namespace PictureLibrary.Client.Clients.Libraries
{
    public class LibrariesClient(IApiHttpClient apiHttpClient)
    {
        public async Task<Library> GetLibrary(string id)
        {
            return await apiHttpClient.Get<Library>($"library/get/{id}");
        }
    }
}
