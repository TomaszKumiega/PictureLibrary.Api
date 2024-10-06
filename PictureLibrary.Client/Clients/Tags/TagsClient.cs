using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.Model;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Tags
{
    internal class TagsClient(IApiHttpClient client) : ITagsClient
    {
        public async Task<TagsDto> GetAllTags(string libraryId, AuthorizationData authorizationData)
        {
            return await client.Get<TagsDto>($"tag/getall?libraryId={libraryId}", authorizationData);
        }

        public async Task<TagDto> AddTag(string libraryId, NewTagDto request, AuthorizationData authorizationData)
        {
            return await client.Post<TagDto>($"tag/add?libraryId={libraryId}", request, authorizationData);
        }

        public async Task<TagDto> UpdateTag(string libraryId, UpdateTagDto request, AuthorizationData authorizationData)
        {
            return await client.Patch<TagDto>($"tag/update?libraryId={libraryId}", request, authorizationData);
        }

        public async Task DeleteTag(string tagId, AuthorizationData authorizationData)
        {
            await client.Delete($"tag/delete?tagId={tagId}", authorizationData);
        }
    }
}
