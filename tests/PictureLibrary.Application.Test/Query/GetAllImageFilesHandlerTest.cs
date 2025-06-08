using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Query.GetAllImageFiles;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Query;

public class GetAllImageFilesHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
    private readonly Mock<IImageFileRepository> _imageFileRepositoryMock;

    private readonly GetAllImageFilesHandler _handler;

    public GetAllImageFilesHandlerTest()
    {
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);
        _imageFileRepositoryMock = new Mock<IImageFileRepository>(MockBehavior.Strict);

        _handler = new GetAllImageFilesHandler(_mapperMock.Object, _libraryRepositoryMock.Object, _imageFileRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Own_Library()
    {
        ObjectId userId = ObjectId.GenerateNewId();
        ObjectId libraryId = ObjectId.GenerateNewId();

        var query = new GetAllImageFilesQuery(userId.ToString(), libraryId.ToString());

        _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
            .ReturnsAsync(false)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));

        _libraryRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_Should_Get_All_Image_Files()
    {
        ObjectId userId = ObjectId.GenerateNewId();
        ObjectId libraryId = ObjectId.GenerateNewId();

        var query = new GetAllImageFilesQuery(userId.ToString(), libraryId.ToString());
        var imageFiles = new ImageFileFaker().Generate(10);
        var imageFileDto = new ImageFileDtoFaker().Generate();

        _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
            .ReturnsAsync(true);

        _imageFileRepositoryMock.Setup(x => x.GetAllFromLibrary(It.IsAny<ObjectId>()))
            .Returns(imageFiles)
            .Verifiable();

        _mapperMock.Setup(x => x.MapToDto(It.IsIn<ImageFile>(imageFiles)))
            .Returns(imageFileDto)
            .Verifiable();

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.ImageFiles.Should().NotBeNullOrEmpty();
        result.ImageFiles.Should().HaveCount(imageFiles.Count);
            
        foreach (var imageFile in result.ImageFiles)
        {
            imageFile.Should().Be(imageFileDto);
        }

        _mapperMock.Verify();
        _imageFileRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_There_Are_Not_ImageFiles()
    {
        ObjectId userId = ObjectId.GenerateNewId();
        ObjectId libraryId = ObjectId.GenerateNewId();

        var query = new GetAllImageFilesQuery(userId.ToString(), libraryId.ToString());

        _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
            .ReturnsAsync(true);

        _imageFileRepositoryMock.Setup(x => x.GetAllFromLibrary(It.IsAny<ObjectId>()))
            .Returns([])
            .Verifiable();

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.ImageFiles.Should().BeEmpty();

        _imageFileRepositoryMock.Verify();
    }
}