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
        private readonly Mock<IImageFileFullInformationProvider> _fullInformationProviderMock;

        private readonly ImageFileUpdateService _updateService;

        public ImageFileUpdateServiceTest()
        {
            _fileServiceMock = new Mock<IFileService>();
            _fullInformationProviderMock = new Mock<IImageFileFullInformationProvider>();

            _updateService = new ImageFileUpdateService(_fileServiceMock.Object, _fullInformationProviderMock.Object);
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
                FileName = "newFileName"
            };

            FileMetadata fileMetadata = new FileMetadataFaker().Generate();
            ImageFile imageFile = new ImageFileFaker().Generate();
            string newFilePath = $"C:\\Folder1\\Folder2\\{updateImageFileData.FileName}";

            FullImageFileInformation fullImageFileInformation = new()
            {
                Id = imageFile.Id.ToString(),
                LibraryId = imageFile.LibraryId.ToString(),
                FileName = updateImageFileData.FileName,
                Base64Thumbnail = string.Empty,
                TagIds = imageFile.TagIds?.Select(x => x.ToString())
            };

            _fileServiceMock.Setup(x => x.ChangeFileName(fileMetadata.FilePath, updateImageFileData.FileName))
                .Returns(newFilePath);

            _fullInformationProviderMock.Setup(x => x.GetFullImageFileInformation(imageFile, fileMetadata))
                .Returns(fullImageFileInformation);

            UpdateImageFileResult result = _updateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            result.Should().NotBeNull();
            result.FileMetadata.FileName.Should().Be(updateImageFileData.FileName);
            result.FileMetadata.FilePath.Should().Be(newFilePath);
            result.FullImageFileInformation.Should().Be(fullImageFileInformation);

            _fileServiceMock.Verify(x => x.ChangeFileName(fileMetadata.FilePath, updateImageFileData.FileName), Times.Once);
            _fullInformationProviderMock.Verify(x => x.GetFullImageFileInformation(imageFile, fileMetadata), Times.Once);
        }

        [Fact]
        public void UpdateImageFile_UpdatesTagIds_WhenNewTagIdsAreSpecified()
        {
            UpdateImageFileData updateImageFileData = new()
            {
                TagIds = ["1", "2", "3"]
            };

            FileMetadata fileMetadata = new FileMetadataFaker().Generate();
            ImageFile imageFile = new ImageFileFaker().Generate();

            FullImageFileInformation fullImageFileInformation = new()
            {
                Id = imageFile.Id.ToString(),
                LibraryId = imageFile.LibraryId.ToString(),
                FileName = fileMetadata.FileName,
                Base64Thumbnail = string.Empty,
                TagIds = updateImageFileData.TagIds?.Select(x => x.ToString())
            };

            _fullInformationProviderMock.Setup(x => x.GetFullImageFileInformation(imageFile, fileMetadata))
                .Returns(fullImageFileInformation);

            UpdateImageFileResult result = _updateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            result.Should().NotBeNull();
            result.ImageFile.TagIds.Should().BeEquivalentTo(updateImageFileData.TagIds?.Select(x => ObjectId.Parse(x)));
            result.FullImageFileInformation.Should().Be(fullImageFileInformation);

            _fullInformationProviderMock.Verify(x => x.GetFullImageFileInformation(imageFile, fileMetadata), Times.Once);
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

            FullImageFileInformation fullImageFileInformation = new()
            {
                Id = imageFile.Id.ToString(),
                LibraryId = updateImageFileData.LibraryId,
                FileName = fileMetadata.FileName,
                Base64Thumbnail = string.Empty,
                TagIds = imageFile.TagIds?.Select(x => x.ToString())
            };

            _fullInformationProviderMock.Setup(x => x.GetFullImageFileInformation(imageFile, fileMetadata))
                .Returns(fullImageFileInformation);

            UpdateImageFileResult result = _updateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            result.Should().NotBeNull();
            result.ImageFile.LibraryId.Should().Be(ObjectId.Parse(updateImageFileData.LibraryId));
            result.FullImageFileInformation.Should().Be(fullImageFileInformation);

            _fullInformationProviderMock.Verify(x => x.GetFullImageFileInformation(imageFile, fileMetadata), Times.Once);
        }
    }
}
