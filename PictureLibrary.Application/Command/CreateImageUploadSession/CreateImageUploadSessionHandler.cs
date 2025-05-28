using MediatR;
using MongoDB.Bson;
using PictureLibrary.Contracts.Results;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command;

public class CreateImageUploadSessionHandler(
    IImageFileRepository imageFileRepository,
    IUploadSessionRepository uploadSessionRepository) 
    : IRequestHandler<CreateImageUploadSessionCommand, CreateImageUploadSessionResult>
{
    public async Task<CreateImageUploadSessionResult> Handle(CreateImageUploadSessionCommand request, CancellationToken cancellationToken)
    {
        UploadSession uploadSession = new()
        {
            Id = ObjectId.GenerateNewId(),
            UserId = ObjectId.Parse(request.UserId),
            FileName = request.CreateUploadSessionDto.FileName,
            FileLength = request.CreateUploadSessionDto.FileLength,
            MissingRanges = $"1-{request.CreateUploadSessionDto.FileLength}",
        };

        ImageFile imageFile = new()
        {
            Id = ObjectId.GenerateNewId(),
            UploadSessionId = uploadSession.Id,
            LibraryId = ObjectId.Parse(request.CreateUploadSessionDto.LibraryId),
            TagIds = request.CreateUploadSessionDto.TagIds.Select(ObjectId.Parse),
        };

        await uploadSessionRepository.Add(uploadSession);
        await imageFileRepository.Add(imageFile);
            
        return new CreateImageUploadSessionResult
        {
            UploadSessionId = uploadSession.Id.ToString(),
        };
    }
}