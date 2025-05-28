using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services;

public class ByteRangesService : IByteRangesService
{
    public IEnumerable<ByteRange> Except(ByteRange range, ByteRange excludedRange)
    {
        bool coversLowerBoundary = range.From == excludedRange.From;
        bool coversUpperBoundary = excludedRange.To == range.To || (excludedRange.To == null && range.To != null) || excludedRange.To > range.To;

        if (coversLowerBoundary && coversUpperBoundary)
        {
            return [];
        }
        else if (coversLowerBoundary && !coversUpperBoundary)
        {
            return [new ByteRange(excludedRange.To!.Value + 1, range.To)];
        }
        else if (!coversLowerBoundary && coversUpperBoundary)
        {
            return [new ByteRange(range.From, excludedRange.From - 1)];
        }
        else
        {
            return [new ByteRange(range.From, excludedRange.From - 1), new ByteRange(excludedRange.To!.Value + 1, range.To)];
        }
    }

    public bool IsOneRangeIncludedInAnother(ByteRange smallerRange, ByteRange biggerRange)
    {
        return smallerRange.From >= biggerRange.From &&
               ((smallerRange.To == null && biggerRange.To == null) || (smallerRange.To <= biggerRange.To));
    }
}