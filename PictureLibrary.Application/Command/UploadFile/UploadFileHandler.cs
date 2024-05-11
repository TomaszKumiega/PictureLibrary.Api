using MediatR;
using MongoDB.Bson;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand, UploadFileResult>
    {
        private readonly IFileService _fileService;
        private readonly IMissingRangesService _missingRangesService;
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public UploadFileHandler(
            IFileService fileService,
            IMissingRangesService missingRangesService,
            IUploadSessionRepository uploadSessionRepository)
        {
            _fileService = fileService;
            _missingRangesService = missingRangesService;
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

            if (request.ContentRange.From == null || request.ContentRange.To == null)
            {
                throw new ArgumentException("Invalid range", nameof(request));
            }

            string requestRange = request.ContentRange.ToString();

            (bool isRangeValid, bool isLastRange) = _missingRangesService.IsIncludedInMissingRanges(uploadSession.MissingRanges, requestRange);

            if (!isRangeValid)
            {
                throw new ArgumentException("Invalid range", nameof(request));
            }

            if (isLastRange)
            {
                _fileService.AppendFile(uploadSession.FileName, request.ContentStream);
            }
            else
            {
                _fileService.Insert(uploadSession.FileName, request.ContentStream, request.ContentRange.From.Value);
            }

            uploadSession.MissingRanges = _missingRangesService.RemoveRangeFromMissingRanges(uploadSession.MissingRanges, requestRange);
            
            bool isUploadFinished = string.IsNullOrEmpty(uploadSession.MissingRanges);

            if (isUploadFinished)
            {
                await _uploadSessionRepository.Delete(uploadSession);

                var result = new FileCreatedResult()
                {
                    
                };
            }
            else
            {
                await _uploadSessionRepository.Update(uploadSession);
            }
        }
    }
}
