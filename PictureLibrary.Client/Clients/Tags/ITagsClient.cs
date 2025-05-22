using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Tags
{
    public interface ITagsClient
    {
        Task<TagDto> AddTag(string libraryId, NewTagDto request);
        Task DeleteTag(string tagId);
        Task<TagsDto> GetAllTags(string libraryId);
        Task<TagDto> UpdateTag(string libraryId, UpdateTagDto request);
    }
}