namespace PictureLibrary.Domain.Services
{
    public interface IMissingRangesParser
    {
        MissingRanges Parse(string ranges);
        string ToString(MissingRanges missingRanges);
    }
}
