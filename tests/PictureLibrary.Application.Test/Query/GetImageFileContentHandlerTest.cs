using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Query;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Query
{
    public class GetImageFileContentHandlerTest
    {
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
        private readonly Mock<IImageFileRepository> _imageFileRepositoryMock;
        private readonly Mock<IFileMetadataRepository> _fileMetadataRepositoryMock;

        private readonly GetImageFileContentHandler _handler;

        public GetImageFileContentHandlerTest()
        {
            _fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);
            _imageFileRepositoryMock = new Mock<IImageFileRepository>(MockBehavior.Strict);
            _fileMetadataRepositoryMock = new Mock<IFileMetadataRepository>(MockBehavior.Strict);

            _handler = new GetImageFileContentHandler(_fileServiceMock.Object, _libraryRepositoryMock.Object, _imageFileRepositoryMock.Object, _fileMetadataRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Own_Library()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();
            GetImageFileContentQuery query = new(userId.ToString(), imageFileId.ToString());
            ImageFile imageFile = new ImageFileFaker().Generate();

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns(imageFile)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, imageFile.LibraryId))
                .ReturnsAsync(false)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _imageFileRepositoryMock.Verify();
            _libraryRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_ImageFile_Does_Not_Exist()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();
            GetImageFileContentQuery query = new(userId.ToString(), imageFileId.ToString());

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns((ImageFile?)null)
                .Verifiable();
            
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _imageFileRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_FileMetadata_Does_Not_Exist()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();
            GetImageFileContentQuery query = new(userId.ToString(), imageFileId.ToString());
            ImageFile imageFile = new ImageFileFaker().Generate();

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns(imageFile)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, imageFile.LibraryId))
                .ReturnsAsync(true);

            _fileMetadataRepositoryMock.Setup(x => x.FindById(imageFile.FileId))
                .Returns((FileMetadata?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _imageFileRepositoryMock.Verify();
            _fileMetadataRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Return_ImageFile_Content()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();
            GetImageFileContentQuery query = new(userId.ToString(), imageFileId.ToString());
            ImageFile imageFile = new ImageFileFaker().Generate();
            FileMetadata fileMetadata = new FileMetadataFaker().Generate();
            MemoryStream stream = new([5, 2, 5, 6, 16]);
            string contentType = "application/json";

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
               .Returns(imageFile)
               .Verifiable();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, imageFile.LibraryId))
                .ReturnsAsync(true)
                .Verifiable();

            _fileMetadataRepositoryMock.Setup(x => x.FindById(imageFile.FileId))
                .Returns(fileMetadata)
                .Verifiable();

            _fileServiceMock.Setup(x => x.OpenFile(fileMetadata.FilePath))
                .Returns(stream)
                .Verifiable();

            _fileServiceMock.Setup(x => x.GetFileMimeType(fileMetadata.FilePath))
                .Returns(contentType)
                .Verifiable();

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Content.Should().BeSameAs(stream);
            result.ContentType.Should().Be(contentType);

            _imageFileRepositoryMock.Verify();
            _libraryRepositoryMock.Verify();
            _fileMetadataRepositoryMock.Verify();
            _fileServiceMock.Verify();
        }
    }
}
