using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Tags
{
    public interface ITagsClient
    {
        Task<Tag> AddTag(string libraryId, AddTagRequest request, AuthorizationData authorizationData);
        Task DeleteTag(string tagId, AuthorizationData authorizationData);
        Task<AllTags> GetAllTags(string libraryId, AuthorizationData authorizationData);
        Task<Tag> UpdateTag(string libraryId, UpdateTagRequest request, AuthorizationData authorizationData);
    }
}