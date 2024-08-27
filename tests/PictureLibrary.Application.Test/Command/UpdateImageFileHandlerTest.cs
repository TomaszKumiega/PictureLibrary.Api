using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using PictureLibrary.TestTools.Fakers;
using System.Text;

namespace PictureLibrary.Application.Test.Command
{
    public class UpdateImageFileHandlerTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
        private readonly Mock<IImageFileRepository> _imageFileRepositoryMock;
        private readonly Mock<IImageFileUpdateService> _imageFileUpdateServiceMock;
        private readonly Mock<IFileMetadataRepository> _fileMetadataRepositoryMock;
        
        private readonly UpdateImageFileHandler _handler;

        public UpdateImageFileHandlerTest()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);
            _imageFileRepositoryMock = new Mock<IImageFileRepository>(MockBehavior.Strict);
            _imageFileUpdateServiceMock = new Mock<IImageFileUpdateService>(MockBehavior.Strict);
            _fileMetadataRepositoryMock = new Mock<IFileMetadataRepository>(MockBehavior.Strict);

            _handler = new UpdateImageFileHandler(
                _mapperMock.Object, 
                _libraryRepositoryMock.Object, 
                _imageFileRepositoryMock.Object,
                _imageFileUpdateServiceMock.Object,
                _fileMetadataRepositoryMock.Object
                );
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_ImageFile_Was_Not_Found()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();
            var dto = new UpdateImageFileDtoFaker().Generate();

            var command = new UpdateImageFileCommand(userId.ToString(), imageFileId.ToString(), dto);

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns((ImageFile?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _imageFileRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Shoul_Throw_NotFoundException_When_FileMetadata_Was_Not_Found()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();
            var dto = new UpdateImageFileDtoFaker().Generate();
            ImageFile imageFile = new ImageFileFaker().Generate();

            var command = new UpdateImageFileCommand(userId.ToString(), imageFileId.ToString(), dto);

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns(imageFile)
                .Verifiable();

            _fileMetadataRepositoryMock.Setup(x => x.FindById(imageFile.FileId))
                .Returns((FileMetadata?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        
            _imageFileRepositoryMock.Verify();
            _fileMetadataRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Own_The_Library()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();
            var dto = new UpdateImageFileDtoFaker().Generate();
            ImageFile imageFile = new ImageFileFaker().Generate();
            FileMetadata fileMetadata = new FileMetadataFaker().Generate();

            var command = new UpdateImageFileCommand(userId.ToString(), imageFileId.ToString(), dto);

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns(imageFile)
                .Verifiable();

            _fileMetadataRepositoryMock.Setup(x => x.FindById(imageFile.FileId))
                .Returns(fileMetadata)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, imageFile.LibraryId))
                .ReturnsAsync(false)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _imageFileRepositoryMock.Verify();
            _fileMetadataRepositoryMock.Verify();
            _libraryRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Update_ImageFile_And_Return_ImageFileInfo()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId imageFileId = ObjectId.GenerateNewId();

            var dto = new UpdateImageFileDtoFaker().Generate();

            ImageFile imageFile = new ImageFileFaker().Generate();
            FileMetadata fileMetadata = new FileMetadataFaker().Generate();
            UpdateImageFileData updateImageFileData = new()
            {
                FileName = dto.FileName,
                LibraryId = dto.LibraryId,
                TagIds = dto.TagIds
            };

            UpdateImageFileResult updateResult = new UpdateImageFileResult(imageFile, fileMetadata);

            ImageFileDto resultDto = new ImageFileDtoFaker().Generate();

            UpdateImageFileCommand command = new(userId.ToString(), imageFileId.ToString(), dto);

            _imageFileRepositoryMock.Setup(x => x.FindById(imageFileId))
                .Returns(imageFile)
                .Verifiable();

            _fileMetadataRepositoryMock.Setup(x => x.FindById(imageFile.FileId))
                .Returns(fileMetadata)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, imageFile.LibraryId))
                .ReturnsAsync(true)
                .Verifiable();

            _mapperMock.Setup(x => x.MapToUpdateImageFileData(dto))
                .Returns(updateImageFileData)
                .Verifiable();

            _imageFileUpdateServiceMock.Setup(x => x.UpdateImageFile(imageFile, fileMetadata, updateImageFileData))
                .Returns(updateResult)
                .Verifiable();

            _mapperMock.Setup(x => x.MapToDto(updateResult.ImageFile))
                .Returns(resultDto)
                .Verifiable();

            _imageFileRepositoryMock.Setup(x => x.Update(updateResult.ImageFile))
                .Callback<ImageFile>((imgFile) =>
                {
                    imgFile.Should().Be(updateResult.ImageFile);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            _fileMetadataRepositoryMock.Setup(x => x.Update(updateResult.FileMetadata))
                .Callback<FileMetadata>((fMetadata) =>
                {
                    fMetadata.Should().Be(updateResult.FileMetadata);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().Be(resultDto);

            _imageFileRepositoryMock.Verify();
            _fileMetadataRepositoryMock.Verify();
            _libraryRepositoryMock.Verify();
            _mapperMock.Verify();
            _imageFileUpdateServiceMock.Verify();
        }
    }
}
