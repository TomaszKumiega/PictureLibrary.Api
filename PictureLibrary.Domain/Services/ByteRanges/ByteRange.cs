namespace PictureLibrary.Domain.Services
{
    public readonly struct ByteRange(long from, long? to)
    {
        public long From { get; } = from;
        public long? To { get; } = to;
    }
}
