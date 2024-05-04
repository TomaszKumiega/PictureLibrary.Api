using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand, UploadFileResult>
    {
        private readonly IFileService _fileService;
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public UploadFileHandler(
            IFileService fileService,
            IUploadSessionRepository uploadSessionRepository)
        {
            _fileService = fileService;
            _uploadSessionRepository = uploadSessionRepository;
        }

        public Task<UploadFileResult> Handle(UploadFileCommand request, CancellationToken cancellationToken)
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

            return null;
        }
    }
}
