using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Test.Command
{
    public class CreateImageUploadSessionHandlerTest
    {
        private readonly Mock<IUploadSessionRepository> _uploadSessionRepositoryMock;

        private readonly CreateImageUploadSessionHandler _handler;

        public CreateImageUploadSessionHandlerTest()
        {
            _uploadSessionRepositoryMock = new Mock<IUploadSessionRepository>(MockBehavior.Strict);

            _handler = new CreateImageUploadSessionHandler(_uploadSessionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_UploadSession()
        {
            var userId = ObjectId.GenerateNewId();
            var dto = new CreateImageUploadSessionDtoFaker().Generate();
            var command = new CreateImageUploadSessionCommand(userId.ToString(), dto);

            _uploadSessionRepositoryMock.Setup(x => x.Add(It.IsAny<UploadSession>()))
                .Callback<UploadSession>(u =>
                {
                    u.Should().NotBeNull();
                    u.UserId.Should().Be(userId);
                    u.FileLength.Should().Be(dto.FileLength);
                    u.FileName.Should().Be(dto.FileName);
                    u.MissingRanges.Should().BeEmpty();
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
        }
    }
}
