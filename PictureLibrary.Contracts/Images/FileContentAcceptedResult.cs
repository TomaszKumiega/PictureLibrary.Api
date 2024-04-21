namespace PictureLibrary.Contracts
{
    public class FileContentAcceptedResult
    {
        public Guid UploadSessionId { get; set; }
        public List<string>? ExpectedRanges { get; set; }
    }
}
