using System.Net.Http.Headers;

namespace PictureLibrary.Domain.Services
{
    public interface IMissingRangesService
    {
        int GetFirstIndexOfMissingRangeContainingAnotherRange(MissingRanges missingRanges, ContentRangeHeaderValue range);
        MissingRanges RemoveRangeFromMissingRanges(MissingRanges missingRanges, ContentRangeHeaderValue range);
    }
}
