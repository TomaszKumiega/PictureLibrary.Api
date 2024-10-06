using PictureLibrary.Client.Model;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Tags
{
    public interface ITagsClient
    {
        Task<TagDto> AddTag(string libraryId, NewTagDto request, AuthorizationData authorizationData);
        Task DeleteTag(string tagId, AuthorizationData authorizationData);
        Task<TagsDto> GetAllTags(string libraryId, AuthorizationData authorizationData);
        Task<TagDto> UpdateTag(string libraryId, UpdateTagDto request, AuthorizationData authorizationData);
    }
}