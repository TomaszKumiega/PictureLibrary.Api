using FluentAssertions;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Services;

namespace PictureLibrary.Infrastructure.Test.ByteRanges
{
    public class ByteRangesServiceTest
    {
        [Fact]
        public void IsOneRangeIncludedInAnother_Returns_True()
        {
            IByteRangesService service = GetByteRangesService();

            service.IsOneRangeIncludedInAnother(new ByteRange(0, 10), new ByteRange(0, 20)).Should().BeTrue();
            service.IsOneRangeIncludedInAnother(new ByteRange(7, 10), new ByteRange(5, 10)).Should().BeTrue();
            service.IsOneRangeIncludedInAnother(new ByteRange(1, 3), new ByteRange(0, 10)).Should().BeTrue();
        }

        [Fact]
        public void IsOneRangeIncludedInAnother_Returns_False()
        {
            IByteRangesService service = GetByteRangesService();

            service.IsOneRangeIncludedInAnother(new ByteRange(0, 21), new ByteRange(0, 20)).Should().BeFalse();
            service.IsOneRangeIncludedInAnother(new ByteRange(3, 10), new ByteRange(5, 10)).Should().BeFalse();
            service.IsOneRangeIncludedInAnother(new ByteRange(3, 12), new ByteRange(3, 10)).Should().BeFalse();
            service.IsOneRangeIncludedInAnother(new ByteRange(1, 12), new ByteRange(3, 10)).Should().BeFalse();
        }

        [Fact]
        public void Except_Returns_Range()
        {
            IByteRangesService service = GetByteRangesService();

            service.Except(new ByteRange(0, 10), new ByteRange(0, 5)).Should().Equal(new ByteRange(6, 10));
            service.Except(new ByteRange(0, 10), new ByteRange(5, 10)).Should().Equal(new ByteRange(0, 4));
            service.Except(new ByteRange(0, 10), new ByteRange(3, 7)).Should().Equal(new ByteRange(0, 2), new ByteRange(8, 10));
        }

        [Fact]
        public void Except_Returns_EmptyCollection()
        {
            IByteRangesService service = GetByteRangesService();

            service.Except(new ByteRange(0, 10), new ByteRange(0, 10)).Should().BeEmpty();
            service.Except(new ByteRange(1, 5), new ByteRange(1, 5)).Should().BeEmpty();
        }

        private static IByteRangesService GetByteRangesService()
        {
            return new ByteRangesService();
        }
    }
}
