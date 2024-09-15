using PictureLibrary.Client.Clients.Libraries;
using PictureLibrary.Client.Clients.Tags;
using PictureLibrary.Client.Clients.Users;

namespace PictureLibrary.Client
{
    internal class PictureLibraryApiClient(
        ITagsClient tagsClient,
        IUsersClient usersClient,
        ILibrariesClient librariesClient) : IPictureLibraryApiClient
    {
        public IUsersClient Users()
        {
            return usersClient;
        }

        public ITagsClient Tags()
        {
            return tagsClient;
        }

        public ILibrariesClient Libraries()
        {
            return librariesClient;
        }
    }
}
