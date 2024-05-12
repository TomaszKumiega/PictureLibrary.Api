using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Services;
using System.Net.Http.Headers;

namespace PictureLibrary.Infrastructure.Test.FileUpload
{
    public class FileUploadServiceTest
    {
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IMissingRangesParser> _missingRangesParserMock;
        private readonly Mock<IMissingRangesService> _missingRangesServiceMock;
        private readonly Mock<IFileMetadataRepository> _fileMetadataRepositoryMock;
        private readonly Mock<IUploadSessionRepository> _uploadSessionRepositoryMock;

        public FileUploadServiceTest()
        {
            _fileServiceMock = new Mock<IFileService>(MockBehavior.Strict);
            _missingRangesParserMock = new Mock<IMissingRangesParser>(MockBehavior.Strict);
            _missingRangesServiceMock = new Mock<IMissingRangesService>(MockBehavior.Strict);
            _fileMetadataRepositoryMock = new Mock<IFileMetadataRepository>(MockBehavior.Strict);
            _uploadSessionRepositoryMock = new Mock<IUploadSessionRepository>(MockBehavior.Strict);
        }

        [Fact]
        public void ShouldFileBeAppended_ReturnsTrue()
        {
            var missingRanges = new MissingRanges([new ByteRange(1, 2), new ByteRange(5, 7), new ByteRange(12, 15)]);
            var range = new ContentRangeHeaderValue(12, 15);

            _missingRangesServiceMock.Setup(x => x.GetFirstIndexOfMissingRangeContainingAnotherRange(missingRanges, range))
                .Returns(2)
                .Verifiable();

            var fileUploadService = GetFileUploadService();

            var result = fileUploadService.ShouldFileBeAppended(missingRanges, range);

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldFileBeAppended_ReturnsFalse()
        {
            var missingRanges = new MissingRanges([new ByteRange(1, 2), new ByteRange(5, 7), new ByteRange(12, 15)]);
            var range = new ContentRangeHeaderValue(1, 2);

            _missingRangesServiceMock.Setup(x => x.GetFirstIndexOfMissingRangeContainingAnotherRange(missingRanges, range))
                .Returns(0)
                .Verifiable();

            var fileUploadService = GetFileUploadService();

            var result = fileUploadService.ShouldFileBeAppended(missingRanges, range);

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldFileBeAppended_ThrowsArgumentNullException()
        {
            var missingRanges = new MissingRanges([new ByteRange(1, 2), new ByteRange(5, 7), new ByteRange(12, 15)]);

            var fileUploadService = GetFileUploadService();

            Assert.Throws<ArgumentNullException>(() => fileUploadService.ShouldFileBeAppended(missingRanges, null));
            Assert.Throws<ArgumentNullException>(() => fileUploadService.ShouldFileBeAppended(missingRanges, new ContentRangeHeaderValue(20)));
        }

        [Fact]
        public async Task AppendBytesToFile_ShouldAppendFile()
        {
            var uploadSession = new UploadSession()
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty
            };
            using var stream = new MemoryStream();

            _fileServiceMock.Setup(x => x.AppendFile(uploadSession.FileName, stream))
                .Verifiable();

            var fileUploadService = GetFileUploadService();

            await fileUploadService.AppendBytesToFile(uploadSession, stream);

            _fileServiceMock.Verify();
        }

        [Fact]
        public async Task AppendBytesToFile_ThrowsArgumentNullException()
        {
            var uploadSession = new UploadSession()
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty
            };
            using var stream = new MemoryStream();

            var fileUploadService = GetFileUploadService();

            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.AppendBytesToFile(null, stream));
            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.AppendBytesToFile(uploadSession, null));
        }

        [Fact]
        public async Task InsertBytesToFile_ShouldInsertBytesToFile()
        {
            var uploadSession = new UploadSession()
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty
            };
            using var stream = new MemoryStream();
            long position = 15;

            _fileServiceMock.Setup(x => x.Insert(uploadSession.FileName, stream, position))
                .Verifiable();

            var fileUploadService = GetFileUploadService();

            await fileUploadService.InsertBytesToFile(uploadSession, stream, position);

            _fileServiceMock.Verify();
        }

        [Fact]
        public async Task InsertBytesToFile_ThrowsArgumentNullException()
        {
            var uploadSession = new UploadSession()
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty
            };
            using var stream = new MemoryStream();

            var fileUploadService = GetFileUploadService();

            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.InsertBytesToFile(null, stream, 5));
            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.InsertBytesToFile(uploadSession, null, 5));
        }

        [Fact]
        public async Task UpdateUploadSession_ShouldUpdateUploadSession_WhenUploadIsntFinished()
        {
            var uploadSession = new UploadSession()
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty
            };
            var missingRange = new MissingRanges([new ByteRange(1, 5)]);
            var newMissingRange = new MissingRanges([new ByteRange(6, 7)]);
            var header = new ContentRangeHeaderValue(1, 2);
            var headerWithNullFrom = new ContentRangeHeaderValue(2);
            var expectedRanges = "1-2, 5-7, 12-15";

            _missingRangesParserMock.Setup(x => x.ToString(newMissingRange))
                .Returns(expectedRanges)
                .Verifiable();

            _missingRangesServiceMock.Setup(x => x.RemoveRangeFromMissingRanges(missingRange, header))
                .Returns(newMissingRange)
                .Verifiable();

            _uploadSessionRepositoryMock.Setup(x => x.Update(uploadSession))
                .Callback((UploadSession up) => up.MissingRanges.Should().Be(expectedRanges))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var fileUploadService = GetFileUploadService();

            await fileUploadService.UpdateUploadSession(uploadSession, missingRange, header);

            _uploadSessionRepositoryMock.Verify();
            _missingRangesServiceMock.Verify();
            _missingRangesParserMock.Verify();
        }

        [Fact]
        public async Task UpdateUploadSession_ShouldDeleteUploadSession_WhenUploadIsFinished()
        {
            var uploadSession = new UploadSession()
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty
            };
            var missingRange = new MissingRanges([new ByteRange(1, 5)]);
            var newMissingRange = new MissingRanges([new ByteRange(6, 7)]);
            var header = new ContentRangeHeaderValue(1, 2);
            var headerWithNullFrom = new ContentRangeHeaderValue(2);

            _missingRangesParserMock.Setup(x => x.ToString(newMissingRange))
                .Returns(string.Empty)
                .Verifiable();

            _missingRangesServiceMock.Setup(x => x.RemoveRangeFromMissingRanges(missingRange, header))
                .Returns(newMissingRange)
                .Verifiable();

            _uploadSessionRepositoryMock.Setup(x => x.Delete(uploadSession))
                .Callback((UploadSession up) => up.MissingRanges.Should().BeEmpty())
                .Returns(Task.CompletedTask)
                .Verifiable();

            var fileUploadService = GetFileUploadService();

            await fileUploadService.UpdateUploadSession(uploadSession, missingRange, header);

            _uploadSessionRepositoryMock.Verify();
            _missingRangesServiceMock.Verify();
            _missingRangesParserMock.Verify();
        }

        [Fact]
        public async Task UpdateUploadSession_ThrowsArgumentNullException()
        {
            var uploadSession = new UploadSession()
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty
            };
            var missingRange = new MissingRanges();
            var header = new ContentRangeHeaderValue(1, 2);
            var headerWithNullFrom = new ContentRangeHeaderValue(2);

            var fileUploadService = GetFileUploadService();

            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.UpdateUploadSession(null, missingRange, header));
            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.UpdateUploadSession(uploadSession, missingRange, null));
            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.UpdateUploadSession(uploadSession, missingRange, headerWithNullFrom));
        }

        [Fact]
        public void IsUploadFinished_ReturnsTrue()
        {
            var uploadSession = new UploadSession() 
            { 
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = string.Empty 
            };

            var fileUploadService = GetFileUploadService();

            var result = fileUploadService.IsUploadFinished(uploadSession);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsUploadFinished_ReturnsFalse()
        {
            var uploadSession = new UploadSession() 
            {
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = "1-2, 5-7, 12-15" 
            };

            var fileUploadService = GetFileUploadService();

            var result = fileUploadService.IsUploadFinished(uploadSession);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsUploadFinished_ThrowsArgumentNullException()
        {
            var fileUploadService = GetFileUploadService();

            Assert.Throws<ArgumentNullException>(() => fileUploadService.IsUploadFinished(null));
        }   

        [Fact]
        public async Task AddFileMetadata_ShouldAddFileMetadata()
        {
            var uploadSession = new UploadSession()
            {
                UserId = ObjectId.GenerateNewId(),
                FileLength = default,
                FileName = string.Empty,
                MissingRanges = "1-2, 5-7, 12-15"
            };

            var fileUploadService = GetFileUploadService();

            _fileMetadataRepositoryMock.Setup(x => x.Add(It.IsAny<FileMetadata>()))
                .Callback((FileMetadata fileMetadata) =>
                {
                    fileMetadata.OwnerId.Should().Be(uploadSession.UserId);
                    fileMetadata.FilePath.Should().Be(uploadSession.FileName);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            await fileUploadService.AddFileMetadata(uploadSession);

            _fileMetadataRepositoryMock.Verify();
        }

        [Fact]
        public async Task AddFileMetadata_ThrowsArgumentNullException()
        {
            var fileUploadService = GetFileUploadService();

            await Assert.ThrowsAsync<ArgumentNullException>(() => fileUploadService.AddFileMetadata(null));
        }

        private IFileUploadService GetFileUploadService()
        {
            return new FileUploadService(_fileServiceMock.Object, _missingRangesParserMock.Object, _missingRangesServiceMock.Object, _fileMetadataRepositoryMock.Object, _uploadSessionRepositoryMock.Object);
        }
    }
}
