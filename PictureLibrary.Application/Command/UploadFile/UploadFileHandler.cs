using MediatR;
using MongoDB.Bson;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class UploadFileHandler(
        IFileUploadService fileUploadService,
        IMissingRangesParser missingRangesParser,
        IImageFileRepository imageFileRepository,
        IUploadSessionRepository uploadSessionRepository) : IRequestHandler<UploadFileCommand, UploadFileResult>
    {
        public async Task<UploadFileResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId uploadSessionId = ObjectId.Parse(request.UploadSessionId);

            var uploadSession = uploadSessionRepository
                .Query()
                .SingleOrDefault(x => x.Id == uploadSessionId) ?? throw new NotFoundException();

            if (uploadSession.UserId != userId)
            {
                throw new NotFoundException();
            }

            MissingRanges missingRanges = missingRangesParser.Parse(uploadSession.MissingRanges);

            bool shouldFileBeAppended = fileUploadService.ShouldFileBeAppended(missingRanges, request.ContentRange);
            
            if (shouldFileBeAppended)
            {
                await fileUploadService.AppendBytesToFile(uploadSession, request.ContentStream);
            }
            else
            {
                await fileUploadService.InsertBytesToFile(uploadSession, request.ContentStream, request.ContentRange.From!.Value);
            }

            await fileUploadService.UpdateUploadSession(uploadSession, missingRanges, request.ContentRange);

            if (fileUploadService.IsUploadFinished(uploadSession))
            {
                await uploadSessionRepository.Delete(uploadSession);

                FileMetadata fileMetadata = await fileUploadService.AddFileMetadata(uploadSession);

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
            return imageFileRepository.Query().Single(x => x.UploadSessionId == uploadSessionId);
        }

        private async Task<ImageFile> UpdateImageFile(FileMetadata fileMetadata, ImageFile imageFile)
        {
            imageFile.FileId = fileMetadata.Id;
            imageFile.UploadSessionId = ObjectId.Empty;

            await imageFileRepository.Update(imageFile);

            return imageFile;
        }

    }
}
