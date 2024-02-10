namespace PictureLibrary.Contracts
{
    public class TagDto
    {
        public required string Id { get; set; }
        public required string LibraryId { get; set; }
        public required string Name { get; set; }
        public required string ColorHex { get; set; }
    }
}
