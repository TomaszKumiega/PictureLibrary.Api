using PictureLibrary.Client.Model;

namespace PictureLibrary.Client.Clients.Tags
{
    public class TagsClient(IApiHttpClient client)
    {
        public async Task<AllTags> GetAllTags(string libraryId, AuthorizationData authorizationData)
        {
            return await client.Get<AllTags>($"tags/getall?libraryId={libraryId}", authorizationData);
        }
    }
}
