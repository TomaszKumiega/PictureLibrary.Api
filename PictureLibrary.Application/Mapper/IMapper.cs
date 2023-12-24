using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Application.Mapper
{
    public interface IMapper
    {
        public LibraryDto MapToDto(Library library);
    }
}
