using FluentAssertions;
using Moq;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Services;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Infrastructure.Test;

public class FileMetadataUpdateServiceTest
{
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly IFileMetadataUpdateService _fileMetadataUpdateService;

    public FileMetadataUpdateServiceTest()
    {
        _fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);

        _fileMetadataUpdateService = new FileMetadataUpdateService(_fileServiceMock.Object);
    }

    [Fact]
    public void UpdateFileName_Should_Throw_ArgumentNullException_When_FileMetadata_Is_Null()
    {
        FileMetadata fileMetadata = null!;
        string newFileName = "newFileName";

        Assert.Throws<ArgumentNullException>(() => _fileMetadataUpdateService.UpdateFileName(fileMetadata, newFileName));
    }

    [Fact]
    public void UpdateFileName_Should_Throw_ArgumentNullException_When_NewFileName_Is_Null()
    {
        FileMetadata fileMetadata = new FileMetadataFaker().Generate();
        string newFileName = null!;

        Assert.Throws<ArgumentNullException>(() => _fileMetadataUpdateService.UpdateFileName(fileMetadata, newFileName));
    }

    [Fact]
    public void UpdateFileName_Should_Throw_ArgumentException_When_NewFileName_Is_Empty()
    {
        FileMetadata fileMetadata = new FileMetadataFaker().Generate();
        string newFileName = "";

        Assert.Throws<ArgumentException>(() => _fileMetadataUpdateService.UpdateFileName(fileMetadata, newFileName));
    }

    [Fact]
    public void UpdateFileName_Should_Throw_ArgumentException_When_NewFileName_Is_WhiteSpace()
    {
        FileMetadata fileMetadata = new FileMetadataFaker().Generate();
        string newFileName = " ";

        Assert.Throws<ArgumentException>(() => _fileMetadataUpdateService.UpdateFileName(fileMetadata, newFileName));
    }

    [Fact]
    public void UpdateFileName_Should_Change_FileName()
    {
        string newFileName = "newFileName";
        string newFilePath = "C:\\newFilePath";
        FileMetadata fileMetadata = new FileMetadataFaker().Generate();

        _fileServiceMock.Setup(x => x.ChangeFileName(fileMetadata.FilePath, newFileName))
            .Returns(newFilePath)
            .Verifiable();

        FileMetadata updatedFileMetadata = _fileMetadataUpdateService.UpdateFileName(fileMetadata, newFileName);

        updatedFileMetadata.FileName.Should().Be(newFileName);
        updatedFileMetadata.FilePath.Should().Be(newFilePath);
    }
}