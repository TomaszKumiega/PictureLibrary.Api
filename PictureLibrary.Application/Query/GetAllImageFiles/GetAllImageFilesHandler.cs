using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Images;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query
{
    public class GetAllImageFilesHandler(
        IMapper mapper,
        ILibraryRepository libraryRepository,
        IImageFileRepository imageFileRepository) 
        : IRequestHandler<GetAllImageFilesQuery, ImageFilesDto>
    {
        public async Task<ImageFilesDto> Handle(GetAllImageFilesQuery request, CancellationToken cancellationToken)
        {
            ObjectId libraryId = ObjectId.Parse(request.LibraryId);
            ObjectId userId = ObjectId.Parse(request.UserId);

            bool userOwnsLibrary = await libraryRepository.IsOwner(userId, libraryId);
            if (!userOwnsLibrary)
            {
                throw new NotFoundException();
            }

            IEnumerable<ImageFile> imageFiles = imageFileRepository.GetAllFromLibrary(libraryId);

            IEnumerable<ImageFileDto> imageFilesDto = imageFiles.Select(mapper.MapToDto);

            return new ImageFilesDto(imageFilesDto);
        }
    }
}
