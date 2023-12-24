using PictureLibrary.Application.Dto;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Application.Mapper
{
    public interface IMapper
    {
        public LibraryDto MapToDto(Library library);
    }
}
