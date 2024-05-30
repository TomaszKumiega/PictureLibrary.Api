using FluentAssertions;
using Moq;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Services;

namespace PictureLibrary.Infrastructure.Test
{
    public class FileServiceTest
    {
        private readonly Mock<IFileWrapper> _fileWrapperMock;
        private readonly Mock<IPathsProvider> _pathsProviderMock;

        public FileServiceTest()
        {
            _fileWrapperMock = new Mock<IFileWrapper>(MockBehavior.Strict);
            _pathsProviderMock = new Mock<IPathsProvider>(MockBehavior.Strict);
        }

        [Fact]
        public void AppendFile_ShouldAppendFile()
        {
            var fileName = "fileName";
            var fileBytes = new byte[] { 1, 2, 3, 4, 5 };
            var newbytes = new byte[] { 6, 7, 8, 9, 10 };

            var contentStream = new MemoryStream();
            var fileStream = new MemoryStream();

            contentStream.Write(newbytes, 0, newbytes.Length);
            fileStream.Write(fileBytes, 0, fileBytes.Length);

            _fileWrapperMock.Setup(f => f.Open(fileName, FileMode.Append))
                .Returns(fileStream)
                .Verifiable();

            var fileService = GetFileService();

            fileService.AppendFile(fileName, contentStream);

            var resultArray = fileStream.ToArray();
            
            resultArray.Should().BeEquivalentTo(fileBytes.Concat(newbytes).ToArray());

            _fileWrapperMock.Verify();
        }

        [Fact]
        public void AppendFile_ShouldThrowArgumentException()
        {
            var fileService = GetFileService();
            using var stream = new MemoryStream();

            Assert.Throws<ArgumentNullException>(() => fileService.AppendFile(null, stream));
            Assert.Throws<ArgumentNullException>(() => fileService.AppendFile("fileName", null));
            Assert.Throws<ArgumentException>(() => fileService.AppendFile(string.Empty, stream));
        }

        [Fact]
        public void Insert_ShouldInsertBytesInFile()
        {
            //TODO
            //string tempDirectoryPath = "\\Temp";
            //string filePath = "\\File.jpg";
            //var fileBytes = new byte[] { 1, 2, 3, 4, 5 };
            //var newbytes = new byte[] { 6, 7, 8, 9, 10 };
            //var contentStream = new MemoryStream(newbytes);
            //_pathsProviderMock.Setup(x => x.GetTempDirectoryPath())
            //    .Returns(tempDirectoryPath)
            //    .Verifiable();

            //_fileWrapperMock.Setup(x => x.Delete(It.IsAny<string>()))
            //    .Verifiable();

            //_fileWrapperMock.Setup(x => x.Copy(It.IsAny<string>(), filePath))
            //    .Verifiable();

            //_fileWrapperMock.Setup(x => x.Open(It.IsAny<string>(), It.IsAny<FileMode>()))
            //    .Returns();

            //var fileService = GetFileService();

            //fileService.Insert(filePath, )
        }

        [Fact]
        public void Insert_ShouldThrowArgumentException()
        {
            var fileService = GetFileService();
            using var stream = new MemoryStream();

            Assert.Throws<ArgumentNullException>(() => fileService.Insert(null, stream, 0));
            Assert.Throws<ArgumentNullException>(() => fileService.Insert("fileName", null, 0));
            Assert.Throws<ArgumentException>(() => fileService.Insert(string.Empty, stream, 0));
        }

        private IFileService GetFileService()
        {
            return new FileService(_fileWrapperMock.Object, _pathsProviderMock.Object);
        }
    }
}
