using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace PictureLibrary.Application.Mapper
{
    [Mapper]
    public partial class MapperlyMapper : IMapper
    {
        public partial LibraryDto MapToDto(Library library);
        public partial TagDto MapToDto(Tag tag);
        public partial UserDto MapToDto(User user);
    }
}
