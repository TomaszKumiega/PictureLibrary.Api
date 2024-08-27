using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test
{
    public class DeleteImageFileHandlerTest
    {
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
        private readonly Mock<IImageFileRepository> _imageFileRepositoryMock;
        private readonly Mock<IFileMetadataRepository> _fileMetadataRepositoryMock;

        private readonly DeleteImageFileHandler _handler;

        public DeleteImageFileHandlerTest()
        {
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);
            _imageFileRepositoryMock = new Mock<IImageFileRepository>(MockBehavior.Strict);
            _fileMetadataRepositoryMock = new Mock<IFileMetadataRepository>(MockBehavior.Strict);

            _handler = new DeleteImageFileHandler(_libraryRepositoryMock.Object, _imageFileRepositoryMock.Object, _fileMetadataRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_ImageFile_Does_Not_Exist()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();

            var command = new DeleteImageFileCommand(userId.ToString(), imageFileId.ToString());

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns((ImageFile?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _imageFileRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Is_Not_Owner_Of_ImageFile()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();

            var imageFile = new ImageFile()
            {
                Id = imageFileId,
                LibraryId = ObjectId.GenerateNewId(),
                FileId = ObjectId.GenerateNewId(),
                TagIds = [ObjectId.GenerateNewId()]
            };
            var command = new DeleteImageFileCommand(userId.ToString(), imageFileId.ToString());

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns(imageFile)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, imageFile.LibraryId))
                .ReturnsAsync(false)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _imageFileRepositoryMock.Verify();
            _libraryRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Delete_ImageFile_And_FileMetadata()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();

            var imageFile = new ImageFile()
            {
                Id = imageFileId,
                LibraryId = ObjectId.GenerateNewId(),
                FileId = ObjectId.GenerateNewId(),
                TagIds = [ObjectId.GenerateNewId()]
            };
            var fileMetadata = new FileMetadataFaker().Generate();
            var command = new DeleteImageFileCommand(userId.ToString(), imageFileId.ToString());

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns(imageFile)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, imageFile.LibraryId))
                .ReturnsAsync(true)
                .Verifiable();

            _fileMetadataRepositoryMock.Setup(x => x.FindById(imageFile.FileId))
                .Returns(fileMetadata)
                .Verifiable();

            _fileMetadataRepositoryMock.Setup(x => x.Delete(fileMetadata))
                .Callback<FileMetadata>(f => f.Should().Be(fileMetadata))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _imageFileRepositoryMock.Setup(x => x.Delete(imageFile))
                .Callback<ImageFile>(f => f.Should().Be(imageFile))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _handler.Handle(command, CancellationToken.None);

            _imageFileRepositoryMock.Verify();
            _libraryRepositoryMock.Verify();
            _fileMetadataRepositoryMock.Verify();
        }
    }
}
