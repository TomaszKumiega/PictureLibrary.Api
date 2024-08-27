using FluentAssertions;
using PictureLibrary.Domain.Services;
using PictureLibrary.Infrastructure.Services;

namespace PictureLibrary.Infrastructure.Test.ByteRanges
{
    public class MissingRangesParserTest
    {
        [Fact]
        public void Parse_ReturnsMissingRanges()
        {
            var parser = GetMissingRangesParser();
            var ranges = "0-499,500-999,1000-1499";
            var expectedRanges = new MissingRanges(
            [
                new(0, 499),
                new(500, 999),
                new(1000, 1499)
            ]);

            MissingRanges result = parser.Parse(ranges);

            expectedRanges.Ranges.Should().Equal(result.Ranges);
        }

        [Fact]
        public void Parse_ReturnsMissingRanges_WhenRangesDontHaveUpperBoundary()
        {
            var parser = GetMissingRangesParser();
            var ranges = "0-499,500-999,1000-";
            var expectedRanges = new MissingRanges(
            [
                new(0, 499),
                new(500, 999),
                new(1000, null)
            ]);

            MissingRanges result = parser.Parse(ranges);

            expectedRanges.Ranges.Should().Equal(result.Ranges);
        }

        [Fact]
        public void Parse_Should_ThrowArgumentException_WhenRangesAreEmpty()
        {
            var parser = GetMissingRangesParser();
            var ranges = string.Empty;

            Assert.Throws<ArgumentException>(() => parser.Parse(ranges));
        }

        [Fact]
        public void Parse_Should_ThrowArgumentException_WhenRangesAreNull()
        {
            var parser = GetMissingRangesParser();
            string ranges = null!;

            Assert.Throws<ArgumentNullException>(() => parser.Parse(ranges));
        }

        [Fact]
        public void Parse_Should_ThrowArgumentException_WhenRangesAreInvalid()
        {
            var parser = GetMissingRangesParser();
            var ranges = "[-499,500-;1000-x,";
            
            Assert.Throws<ArgumentException>(() => parser.Parse(ranges));
        }

        [Fact]
        public void ToString_ReturnsString()
        {
            var parser = GetMissingRangesParser();
            var ranges = new MissingRanges(
                           [
                new(0, 499),
                new(500, 999),
                new(1000, 1499)
            ]);

            var expectedString = "0-499,500-999,1000-1499";

            string result = parser.ToString(ranges);

            expectedString.Should().Be(result);
        }

        [Fact]
        public void ToString_ReturnsString_WhenRangesDontHaveUpperBoundary()
        {
            var parser = GetMissingRangesParser();
            var ranges = new MissingRanges(
                                          [
                new(0, 499),
                new(500, 999),
                new(1000, null)
            ]);

            var expectedString = "0-499,500-999,1000-";

            string result = parser.ToString(ranges);

            expectedString.Should().Be(result);
        }

        [Fact]
        public void ToString_ReturnsEmptyString_WhenRangesAreEmpty()
        {
            var parser = GetMissingRangesParser();
            var ranges = new MissingRanges([]);

            var expectedString = string.Empty;

            string result = parser.ToString(ranges);

            expectedString.Should().Be(result);
        }

        private static IMissingRangesParser GetMissingRangesParser()
        {
            return new MissingRangesParser();
        }
    }
}
