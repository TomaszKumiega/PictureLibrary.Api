using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Mapper
{
    public interface IMapper
    {
        public LibraryDto MapToDto(Library library);
        public TagDto MapToDto(Tag tag);
        public UserDto MapToDto(User user);
        public ImageFileDto MapToDto(ImageFile imageFile);
        public UserAuthorizationDataDto MapToDto(AuthorizationData authorizationData);
        public UpdateImageFileData MapToUpdateImageFileData(UpdateImageFileDto dto);
        
    }
}
