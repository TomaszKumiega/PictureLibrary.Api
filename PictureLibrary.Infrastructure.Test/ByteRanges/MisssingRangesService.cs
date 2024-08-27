using FluentAssertions;
using Moq;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Services;
using System.Net.Http.Headers;

namespace PictureLibrary.Infrastructure.Test.ByteRanges
{
    public class MisssingRangesService
    {
        private readonly Mock<IByteRangesService> _byteRangesServiceMock;

        public MisssingRangesService()
        {
            _byteRangesServiceMock = new Mock<IByteRangesService>(MockBehavior.Strict);
        }

        [Fact]
        public void GetFirstIndexOfMissingRangeContainingAnotherRange_ReturnsIndex()
        {
            var byteRange1 = new ByteRange(0, 10);
            var byteRange2 = new ByteRange(20, 30);
            var byteRange3 = new ByteRange(40, 50);

            List<ByteRange> byteRanges = [byteRange1, byteRange2, byteRange3];
            var missingRanges = new MissingRanges(byteRanges);
            var contentRange = new ContentRangeHeaderValue(22, 25);
            var contentByteRange = new ByteRange(22, 25);

            _byteRangesServiceMock.SetupSequence(x => x.IsOneRangeIncludedInAnother(It.IsAny<ByteRange>(), It.IsAny<ByteRange>()))
                .Returns(false)
                .Returns(true)
                .Returns(false);

            var missingRangesService = GetMissingRangesService();

            var result = missingRangesService.GetFirstIndexOfMissingRangeContainingAnotherRange(missingRanges, contentRange);

            result.Should().Be(1);

            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(contentByteRange, byteRange1), Times.Once());
            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(contentByteRange, byteRange2), Times.Once());
            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(contentByteRange, byteRange2), Times.Once());
        }

        [Fact]
        public void GetFirstIndexOfMissingRangeContainingAnotherRange_ReturnsMinusOne()
        {
            List<ByteRange> byteRanges = [new ByteRange(0, 10), new ByteRange(20, 30), new ByteRange(40, 50)];
            var missingRanges = new MissingRanges(byteRanges);
            var contentRange = new ContentRangeHeaderValue(11, 19);

            _byteRangesServiceMock.Setup(x => x.IsOneRangeIncludedInAnother(It.IsAny<ByteRange>(), It.IsAny<ByteRange>()))
                .Returns(false)
                .Verifiable();

            var missingRangesService = GetMissingRangesService();

            var result = missingRangesService.GetFirstIndexOfMissingRangeContainingAnotherRange(missingRanges, contentRange);

            result.Should().Be(-1);

            _byteRangesServiceMock.Verify();
        }

        [Fact]
        public void GetFirstIndexOfMissingRangeContainingAnotherRange_ThrowsArgumentException()
        {
            var missingRangesService = GetMissingRangesService();

            Assert.Throws<ArgumentNullException>(() => missingRangesService.GetFirstIndexOfMissingRangeContainingAnotherRange(new MissingRanges([]), null!));
            Assert.Throws<ArgumentNullException>(() => missingRangesService.GetFirstIndexOfMissingRangeContainingAnotherRange(new MissingRanges([]), new ContentRangeHeaderValue(10)));
        }

        [Fact]
        public void RemoveRangeFromMissingRanges_ShouldReturnMissingRanges_WithoutSpecifiedRange()
        {
            var rangeToExclude = new ContentRangeHeaderValue(10, 19);

            var byteRange1 = new ByteRange(10, 19);
            var byteRange2 = new ByteRange(20, 30);
            var byteRange3 = new ByteRange(40, 50);

            List<ByteRange> missingByteRanges = [byteRange1, byteRange2, byteRange3];
            var missingRanges = new MissingRanges(missingByteRanges);

            _byteRangesServiceMock.SetupSequence(x => x.IsOneRangeIncludedInAnother(It.IsAny<ByteRange>(), It.IsAny<ByteRange>()))
                .Returns(true)
                .Returns(false)
                .Returns(false);

            _byteRangesServiceMock.SetupSequence(x => x.Except(It.IsAny<ByteRange>(), It.IsAny<ByteRange>()))
                .Returns([])
                .Returns([new(20, 30)])
                .Returns([new(40, 50)]);

            var missingRangesService = GetMissingRangesService();

            var result = missingRangesService.RemoveRangeFromMissingRanges(missingRanges, rangeToExclude);

            result.Ranges.Should().BeEquivalentTo(new MissingRanges([new ByteRange(20, 30), new ByteRange(40, 50)]).Ranges);

            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(new ByteRange(10, 19), byteRange1), Times.Once());
            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(new ByteRange(10, 19), byteRange2), Times.Once());
            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(new ByteRange(10, 19), byteRange3), Times.Once());

            _byteRangesServiceMock.Verify(x => x.Except(byteRange1, new ByteRange(10, 19)), Times.Once());
        }

        [Fact]
        public void RemoveRangeFromMissingRanges_ShouldReturnMissingRanges_WhenRangeIsNotPresent()
        {
            var rangeToExclude = new ContentRangeHeaderValue(1, 9);

            var byteRange1 = new ByteRange(10, 19);
            var byteRange2 = new ByteRange(20, 30);
            var byteRange3 = new ByteRange(40, 50);

            List<ByteRange> missingByteRanges = [byteRange1, byteRange2, byteRange3];
            var missingRanges = new MissingRanges(missingByteRanges);

            _byteRangesServiceMock.SetupSequence(x => x.IsOneRangeIncludedInAnother(It.IsAny<ByteRange>(), It.IsAny<ByteRange>()))
                .Returns(false)
                .Returns(false)
                .Returns(false);

            var missingRangesService = GetMissingRangesService();

            var result = missingRangesService.RemoveRangeFromMissingRanges(missingRanges, rangeToExclude);

            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(new ByteRange(1, 9), byteRange1), Times.Once());
            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(new ByteRange(1, 9), byteRange2), Times.Once());
            _byteRangesServiceMock.Verify(x => x.IsOneRangeIncludedInAnother(new ByteRange(1, 9), byteRange3), Times.Once());
        }

        [Fact]
        public void RemoveRangeFromMissingRanges_ThrowsArgumentNullException()
        {
            var missingRangesService = GetMissingRangesService();

            Assert.Throws<ArgumentNullException>(() => missingRangesService.RemoveRangeFromMissingRanges(new MissingRanges([]), null!));
            Assert.Throws<ArgumentNullException>(() => missingRangesService.RemoveRangeFromMissingRanges(new MissingRanges([]), new ContentRangeHeaderValue(10)));
        }

        private IMissingRangesService GetMissingRangesService()
        {
            return new MissingRangesService(_byteRangesServiceMock.Object);
        }
    }
}
