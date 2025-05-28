using PictureLibrary.Domain.Services;
using System.Text.RegularExpressions;

namespace PictureLibrary.Infrastructure.Services;

public partial class MissingRangesParser : IMissingRangesParser
{
    public MissingRanges Parse(string ranges)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ranges);

        List<ByteRange> rangesList = [];
        string[] rangesArray = ranges.Split(',', StringSplitOptions.RemoveEmptyEntries);

        Regex rangeRegex = RangeRegex();

        long from;
        long? to;

        foreach (var range in rangesArray)
        {
            Match match = rangeRegex.Match(range);

            if (!match.Success)
            {
                throw new ArgumentException("Invalid range format.", nameof(ranges));
            }

            from = long.Parse(match.Groups["from"].Value);
            to = match.Groups["to"].Success ? long.Parse(match.Groups["to"].Value) : null;

            rangesList.Add(new ByteRange(from, to));
        }

        return new MissingRanges(rangesList);
    }

    public string ToString(MissingRanges missingRanges)
    {
        IEnumerable<string> ranges = missingRanges.Ranges.Select(x => $"{x.From}-{x.To}");

        return string.Join(',', ranges);
    }

    [GeneratedRegex(@"^(?'from'\d+)-(?'to'\d+)?$")]
    private static partial Regex RangeRegex();
}