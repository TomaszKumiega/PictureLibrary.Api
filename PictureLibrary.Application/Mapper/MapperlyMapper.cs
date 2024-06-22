﻿using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Services;
using Riok.Mapperly.Abstractions;

namespace PictureLibrary.Application.Mapper
{
    [Mapper]
    public partial class MapperlyMapper : IMapper
    {
        public partial LibraryDto MapToDto(Library library);
        public partial TagDto MapToDto(Tag tag);
        public partial UserDto MapToDto(User user);
        public partial UserAuthorizationDataDto MapToDto(AuthorizationData authorizationData);
        public partial ImageFileDto MapToDto(FullImageFileInformation fullImageFileInformation);
        public partial UpdateImageFileData MapToUpdateImageFileData(UpdateImageFileDto dto);
    }
}
