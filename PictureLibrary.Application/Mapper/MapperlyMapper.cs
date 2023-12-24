using PictureLibrary.Application.Dto;
using PictureLibrary.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace PictureLibrary.Application.Mapper
{
    [Mapper]
    public partial class MapperlyMapper : IMapper
    {
        public partial LibraryDto MapToDto(Library library);
    }
}
