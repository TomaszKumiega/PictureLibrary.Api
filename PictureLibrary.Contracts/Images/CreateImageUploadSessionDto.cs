namespace PictureLibrary.Contracts
{
    public class CreateImageUploadSessionDto
    {
        public required string FileName { get; set; }
        public required int FileLength { get; set; }
        public required string LibraryId { get; set; }
        public required IEnumerable<string> TagIds { get; set; }
    }
}
