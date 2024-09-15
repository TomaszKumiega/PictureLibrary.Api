using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Tags
{
    public class TagsClient(IApiHttpClient client)
    {
        public async Task<AllTags> GetAllTags(string libraryId, AuthorizationData authorizationData)
        {
            return await client.Get<AllTags>($"tags/getall?libraryId={libraryId}", authorizationData);
        }

        public async Task<Tag> AddTag(string libraryId, AddTagRequest request, AuthorizationData authorizationData)
        {
            return await client.Post<Tag>($"tags/add?libraryId={libraryId}", request, authorizationData);
        }

        public async Task<Tag> UpdateTag(string libraryId, UpdateTagRequest request, AuthorizationData authorizationData)
        {
            return await client.Post<Tag>($"tags/update?libraryId={libraryId}", request, authorizationData);
        }
    }
}
