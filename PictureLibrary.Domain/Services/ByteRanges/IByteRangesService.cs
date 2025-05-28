namespace PictureLibrary.Domain.Services;

public interface IByteRangesService
{
    bool IsOneRangeIncludedInAnother(ByteRange smallerRange, ByteRange biggerRange);
    IEnumerable<ByteRange> Except(ByteRange range, ByteRange excludedRange);
}