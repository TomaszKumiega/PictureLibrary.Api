using MediatR;
using MongoDB.Bson;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand, UploadFileResult>
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IMissingRangesParser _missingRangesParser;
        private readonly IImageFileRepository _imageFileRepository;
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public UploadFileHandler(
            IFileUploadService fileUploadService,
            IMissingRangesParser missingRangesParser,
            IImageFileRepository imageFileRepository,
            IUploadSessionRepository uploadSessionRepository)
        {
            _fileUploadService = fileUploadService;
            _missingRangesParser = missingRangesParser;
            _imageFileRepository = imageFileRepository;
            _uploadSessionRepository = uploadSessionRepository;
        }

        public async Task<UploadFileResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId uploadSessionId = ObjectId.Parse(request.UploadSessionId);

            var uploadSession = _uploadSessionRepository
                .Query()
                .SingleOrDefault(x => x.Id == uploadSessionId) ?? throw new NotFoundException();

            if (uploadSession.UserId != userId)
            {
                throw new NotFoundException();
            }

            MissingRanges missingRanges = _missingRangesParser.Parse(uploadSession.MissingRanges);

            bool shouldFileBeAppended = _fileUploadService.ShouldFileBeAppended(missingRanges, request.ContentRange);
            
            if (shouldFileBeAppended)
            {
                await _fileUploadService.AppendBytesToFile(uploadSession, request.ContentStream);
            }
            else
            {
                await _fileUploadService.InsertBytesToFile(uploadSession, request.ContentStream, request.ContentRange.From!.Value);
            }

            await _fileUploadService.UpdateUploadSession(uploadSession, missingRanges, request.ContentRange);

            if (_fileUploadService.IsUploadFinished(uploadSession))
            {
                await _uploadSessionRepository.Delete(uploadSession);

                FileMetadata fileMetadata = await _fileUploadService.AddFileMetadata(uploadSession);

                ImageFile imageFile = GetImageFile(uploadSessionId);
                await UpdateImageFile(fileMetadata, imageFile);

                var fileCreatedResult = new FileCreatedResult(imageFile.Id.ToString());
                return new UploadFileResult(fileCreatedResult);
            }
            else
            {
                var fileContentAcceptedResult = new FileContentAcceptedResult(uploadSession.Id.ToString(), uploadSession.MissingRanges);
                return new UploadFileResult(fileContentAcceptedResult);
            }
        }

        private ImageFile GetImageFile(ObjectId uploadSessionId)
        {
            return _imageFileRepository.Query().Single(x => x.UploadSessionId == uploadSessionId);
        }

        private async Task<ImageFile> UpdateImageFile(FileMetadata fileMetadata, ImageFile imageFile)
        {
            imageFile.FileId = fileMetadata.Id;
            imageFile.UploadSessionId = ObjectId.Empty;

            await _imageFileRepository.Update(imageFile);

            return imageFile;
        }

    }
}
