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
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public UploadFileHandler(
            IFileUploadService fileUploadService,
            IMissingRangesParser missingRangesParser,
            IUploadSessionRepository uploadSessionRepository)
        {
            _fileUploadService = fileUploadService;
            _missingRangesParser = missingRangesParser;
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

            FileMetadata fileMetadata = await _fileUploadService.AddFileMetadata(uploadSession);

            if (_fileUploadService.IsUploadFinished(uploadSession))
            {
                var fileCreatedResult = new FileCreatedResult();

                return new UploadFileResult(fileCreatedResult);
            }
            else
            {
                var fileContentAcceptedResult = new FileContentAcceptedResult();

                return new UploadFileResult(fileContentAcceptedResult);
            }
        }
    }
}
