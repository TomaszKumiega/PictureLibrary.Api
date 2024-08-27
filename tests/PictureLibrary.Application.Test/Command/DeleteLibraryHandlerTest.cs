using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Command
{
    public class DeleteLibraryHandlerTest
    {
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;

        private readonly DeleteLibraryHandler _handler;

        public DeleteLibraryHandlerTest()
        {
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

            _handler = new DeleteLibraryHandler(_libraryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Delete_Library()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId libraryId = ObjectId.GenerateNewId();

            var library = new LibraryFaker().Generate();
            var command = new DeleteLibraryCommand(userId.ToString(), libraryId.ToString());

            _libraryRepositoryMock.Setup(x => x.Get(userId, libraryId))
                .ReturnsAsync(library)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.Delete(library))
                .Callback((Library lib) =>
                {
                    lib.Should().Be(library);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _handler.Handle(command, CancellationToken.None);

            _libraryRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Library_Doesnt_Exist()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId libraryId = ObjectId.GenerateNewId();

            var library = new LibraryFaker().Generate();
            var command = new DeleteLibraryCommand(userId.ToString(), libraryId.ToString());

            _libraryRepositoryMock.Setup(x => x.Get(userId, libraryId))
                .ReturnsAsync((Library?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
