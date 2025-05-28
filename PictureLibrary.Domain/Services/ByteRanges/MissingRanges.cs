namespace PictureLibrary.Domain.Services;

public readonly struct MissingRanges(IEnumerable<ByteRange> byteRanges)
{
    public IEnumerable<ByteRange> Ranges { get; } = byteRanges;
}