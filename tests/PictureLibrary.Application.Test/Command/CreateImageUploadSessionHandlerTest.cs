using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.CreateImageUploadSession;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Test.Command;

public class CreateImageUploadSessionHandlerTest
{
    private readonly Mock<IImageFileRepository> _imageFileRepositoryMock;
    private readonly Mock<IUploadSessionRepository> _uploadSessionRepositoryMock;

    private readonly CreateImageUploadSessionHandler _handler;

    public CreateImageUploadSessionHandlerTest()
    {
        _imageFileRepositoryMock = new Mock<IImageFileRepository>(MockBehavior.Strict);
        _uploadSessionRepositoryMock = new Mock<IUploadSessionRepository>(MockBehavior.Strict);

        _handler = new CreateImageUploadSessionHandler(_imageFileRepositoryMock.Object, _uploadSessionRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_UploadSession_And_ImageFile()
    {
        var libraryId = ObjectId.GenerateNewId();
        IEnumerable<ObjectId> tagIds = [ObjectId.GenerateNewId(), ObjectId.GenerateNewId()];

        var userId = ObjectId.GenerateNewId();
        var dto = new CreateImageUploadSessionDtoFaker()
            .WithLibraryId(libraryId)
            .WithTagIds(tagIds)
            .Generate();

        ObjectId uploadSessionId = ObjectId.Empty;

        var command = new CreateImageUploadSessionCommand(userId.ToString(), dto);

        _uploadSessionRepositoryMock.Setup(x => x.Add(It.IsAny<UploadSession>()))
            .Callback<UploadSession>(u =>
            {
                uploadSessionId = u.Id;

                u.Should().NotBeNull();
                u.UserId.Should().Be(userId);
                u.FileLength.Should().Be(dto.FileLength);
                u.FileName.Should().Be(dto.FileName);
                u.MissingRanges.Should().Be($"1-{dto.FileLength}");
            })
            .Returns(Task.CompletedTask)
            .Verifiable();

        _imageFileRepositoryMock.Setup(x => x.Add(It.IsAny<ImageFile>()))
            .Callback<ImageFile>(i =>
            {
                i.Should().NotBeNull();
                i.UploadSessionId.Should().Be(uploadSessionId);
                i.LibraryId.Should().Be(libraryId);
                i.TagIds.Should().BeEquivalentTo(tagIds);
            })
            .Returns(Task.CompletedTask)
            .Verifiable();

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();

        _imageFileRepositoryMock.Verify();
        _uploadSessionRepositoryMock.Verify();
    }
}