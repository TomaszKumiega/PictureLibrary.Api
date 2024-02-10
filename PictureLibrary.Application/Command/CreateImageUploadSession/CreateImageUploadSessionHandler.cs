using MediatR;
using MongoDB.Bson;
using PictureLibrary.Contracts.Results;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class CreateImageUploadSessionHandler : IRequestHandler<CreateImageUploadSessionCommand, CreateImageUploadSessionResult>
    {
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public CreateImageUploadSessionHandler(
            IUploadSessionRepository uploadSessionRepository)
        {
            _uploadSessionRepository = uploadSessionRepository;
        }

        public async Task<CreateImageUploadSessionResult> Handle(CreateImageUploadSessionCommand request, CancellationToken cancellationToken)
        {
            UploadSession uploadSession = new()
            {
                Id = ObjectId.GenerateNewId(),
                UserId = ObjectId.Parse(request.UserId),
                FileName = request.CreateUploadSessionDto.FileName,
                FileLength = request.CreateUploadSessionDto.FileLength,
                MissingRanges = string.Empty,
            };

            await _uploadSessionRepository.Add(uploadSession);

            return new CreateImageUploadSessionResult
            {
                UploadSessionId = uploadSession.Id.ToString(),
            };
        }
    }
}
