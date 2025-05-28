using PictureLibrary.Domain.Services;
using System.Net.Http.Headers;

namespace PictureLibrary.Infrastructure.Services;

public class MissingRangesService(IByteRangesService byteRangesService) : IMissingRangesService
{
    public int GetFirstIndexOfMissingRangeContainingAnotherRange(MissingRanges missingRanges, ContentRangeHeaderValue range)
    {
        ArgumentNullException.ThrowIfNull(range?.From);

        ByteRange byteRange = GetByteRangeFromContentRangeHeaderValue(range);

        var matchingRange = missingRanges.Ranges
            .Select((range, i) => new
            {
                IsInRange = byteRangesService.IsOneRangeIncludedInAnother(byteRange, range),
                Index = i,
            })
            .SingleOrDefault(x => x.IsInRange);

        return matchingRange?.Index ?? -1;
    }

    public MissingRanges RemoveRangeFromMissingRanges(MissingRanges missingRanges, ContentRangeHeaderValue range)
    {
        int index = GetFirstIndexOfMissingRangeContainingAnotherRange(missingRanges, range);

        if (index == -1)
        {
            return missingRanges;
        }

        ByteRange byteRange = GetByteRangeFromContentRangeHeaderValue(range);

        IEnumerable<ByteRange> resultOfExcludingRange = byteRangesService.Except(missingRanges.Ranges.ElementAt(index), byteRange);

        IEnumerable<ByteRange> newRanges = missingRanges.Ranges
            .Take(index)
            .Concat(resultOfExcludingRange)
            .Concat(missingRanges.Ranges.Skip(index + 1));

        return new MissingRanges(newRanges);
    }

    private static ByteRange GetByteRangeFromContentRangeHeaderValue(ContentRangeHeaderValue contentRangeHeaderValue)
    {
        return new(contentRangeHeaderValue.From!.Value, contentRangeHeaderValue.To);
    }
}