using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class UpdateImageFileHandler : IRequestHandler<UpdateImageFileCommand, ImageFileDto>
    {
        private readonly IMapper _mapper;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IImageFileRepository _imageFileRepository;
        private readonly IImageFileUpdateService _imageFileUpdateService;
        private readonly IFileMetadataRepository _fileMetadataRepository;

        public UpdateImageFileHandler(
            IMapper mapper,
            ILibraryRepository libraryRepository,
            IImageFileRepository imageFileRepository,
            IImageFileUpdateService imageFileUpdateService,
            IFileMetadataRepository fileMetadataRepository)
        {
            _mapper = mapper;
            _libraryRepository = libraryRepository;
            _imageFileRepository = imageFileRepository;
            _imageFileUpdateService = imageFileUpdateService;
            _fileMetadataRepository = fileMetadataRepository;
        }

        public async Task<ImageFileDto> Handle(UpdateImageFileCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId imageFileId = ObjectId.Parse(request.ImageFileId);

            ImageFile imageFile = _imageFileRepository.FindById(imageFileId) ?? throw new NotFoundException();
            FileMetadata fileMetadata = _fileMetadataRepository.FindById(imageFile.FileId) ?? throw new NotFoundException();

            await EnsureUserOwnsTheLibrary(userId, imageFile.LibraryId);

            UpdateImageFileData updateImageFileData = _mapper.MapToUpdateImageFileData(request.Dto);
            UpdateImageFileResult result = _imageFileUpdateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            await _fileMetadataRepository.Update(result.FileMetadata);
            await _imageFileRepository.Update(result.ImageFile);

            return _mapper.MapToDto(result.FullImageFileInformation);
        }

        private async Task EnsureUserOwnsTheLibrary(ObjectId userId, ObjectId libraryId)
        {
            bool isUserOwnerOfTheLibrary = await _libraryRepository.IsOwner(userId, libraryId);
            if (!isUserOwnerOfTheLibrary)
            {
                throw new NotFoundException();
            }
        }
    }
}
