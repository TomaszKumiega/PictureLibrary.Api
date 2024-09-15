using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Tags
{
    public class TagsClient(IApiHttpClient client) : ITagsClient
    {
        public async Task<AllTags> GetAllTags(string libraryId, AuthorizationData authorizationData)
        {
            return await client.Get<AllTags>($"tag/getall?libraryId={libraryId}", authorizationData);
        }

        public async Task<Tag> AddTag(string libraryId, AddTagRequest request, AuthorizationData authorizationData)
        {
            return await client.Post<Tag>($"tag/add?libraryId={libraryId}", request, authorizationData);
        }

        public async Task<Tag> UpdateTag(string libraryId, UpdateTagRequest request, AuthorizationData authorizationData)
        {
            return await client.Patch<Tag>($"tag/update?libraryId={libraryId}", request, authorizationData);
        }

        public async Task DeleteTag(string tagId, AuthorizationData authorizationData)
        {
            await client.Get<Tag>($"tag/delete?tagId={tagId}", authorizationData);
        }
    }
}
