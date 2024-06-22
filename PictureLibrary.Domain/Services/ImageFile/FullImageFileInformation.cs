namespace PictureLibrary.Domain.Services
{
    public class FullImageFileInformation
    {
        public required string Id { get; set; }
        public required string FileName { get; set; }
        public required string Base64Thumbnail { get; set; }
        public required string LibraryId { get; set; }
        public required IEnumerable<string>? TagIds { get; set; }
    }
}
