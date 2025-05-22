using PictureLibrary.Client.BaseClient;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Tags
{
    internal class TagsClient(IApiHttpClient client) : ITagsClient
    {
        public async Task<TagsDto> GetAllTags(string libraryId)
        {
            return await client.Get<TagsDto>($"tag/getall?libraryId={libraryId}");
        }

        public async Task<TagDto> AddTag(string libraryId, NewTagDto request)
        {
            return await client.Post<TagDto>($"tag/add?libraryId={libraryId}", request);
        }

        public async Task<TagDto> UpdateTag(string libraryId, UpdateTagDto request)
        {
            return await client.Patch<TagDto>($"tag/update?libraryId={libraryId}", request);
        }

        public async Task DeleteTag(string tagId)
        {
            await client.Delete($"tag/delete?tagId={tagId}");
        }
    }
}
