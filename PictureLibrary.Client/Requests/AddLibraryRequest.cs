namespace PictureLibrary.Client.Requests
{
    public class AddLibraryRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
