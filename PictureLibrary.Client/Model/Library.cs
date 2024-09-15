namespace PictureLibrary.Client.Model
{
    public class Library
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
