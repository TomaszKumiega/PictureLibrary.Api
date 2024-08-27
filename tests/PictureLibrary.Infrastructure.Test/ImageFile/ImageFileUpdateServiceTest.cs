using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Services;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Infrastructure.Test
{
    public class ImageFileUpdateServiceTest
    {
        private readonly Mock<IFileService> _fileServiceMock;

        private readonly ImageFileUpdateService _updateService;

        public ImageFileUpdateServiceTest()
        {
            _fileServiceMock = new Mock<IFileService>();

            _updateService = new ImageFileUpdateService(_fileServiceMock.Object);
        }

        [Fact]
        public void UpdateImageFile_ThrowsArgumentNullException()
        {
            var imageFile = new ImageFileFaker().Generate();
            var fileMetadata = new FileMetadataFaker().Generate();
            var updateImageFileData = new UpdateImageFileData();

            Assert.Throws<ArgumentNullException>(() => _updateService.UpdateImageFile(imageFile, fileMetadata, null!));
            Assert.Throws<ArgumentNullException>(() => _updateService.UpdateImageFile(imageFile, null!, updateImageFileData));
            Assert.Throws<ArgumentNullException>(() => _updateService.UpdateImageFile(null!, fileMetadata, updateImageFileData));
        }

        [Fact]
        public void UpdateImageFile_UpdatesFileName_WhenNewFileNameIsSpecified()
        {
            UpdateImageFileData updateImageFileData = new()
            {
                FileName = "fileName123.jpg"
            };

            string oldFilePath = "C:\\Folder1\\Folder2\\oldFileName.jpg";
            FileMetadata fileMetadata = new FileMetadataFaker().Generate();
            fileMetadata.FilePath = oldFilePath;
            ImageFile imageFile = new ImageFileFaker().Generate();
            string newFilePath = $"C:\\Folder1\\Folder2\\{updateImageFileData.FileName}";

            _fileServiceMock.Setup(x => x.ChangeFileName(fileMetadata.FilePath, updateImageFileData.FileName))
                .Returns(newFilePath);

            UpdateImageFileResult result = _updateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            result.Should().NotBeNull();
            result.FileMetadata.FileName.Should().Be(updateImageFileData.FileName);
            result.FileMetadata.FilePath.Should().Be(newFilePath);
            result.ImageFile.FileName.Should().Be(updateImageFileData.FileName);

            _fileServiceMock.Verify(x => x.ChangeFileName(oldFilePath, updateImageFileData.FileName), Times.Once);
        }

        [Fact]
        public void UpdateImageFile_UpdatesTagIds_WhenNewTagIdsAreSpecified()
        {
            UpdateImageFileData updateImageFileData = new()
            {
                TagIds = [ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString(), ObjectId.GenerateNewId().ToString()]
            };

            FileMetadata fileMetadata = new FileMetadataFaker().Generate();
            ImageFile imageFile = new ImageFileFaker().Generate();

            UpdateImageFileResult result = _updateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            result.Should().NotBeNull();
            result.ImageFile.TagIds.Should().BeEquivalentTo(updateImageFileData.TagIds?.Select(x => ObjectId.Parse(x)));
        }

        [Fact]
        public void UpdateImageFile_UpdatesLibraryId_WhenNewLibraryIdIsSpecified()
        {
            UpdateImageFileData updateImageFileData = new()
            {
                LibraryId = ObjectId.GenerateNewId().ToString()
            };

            FileMetadata fileMetadata = new FileMetadataFaker().Generate();
            ImageFile imageFile = new ImageFileFaker().Generate();

            UpdateImageFileResult result = _updateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            result.Should().NotBeNull();
            result.ImageFile.LibraryId.Should().Be(ObjectId.Parse(updateImageFileData.LibraryId));
        }
    }
}
