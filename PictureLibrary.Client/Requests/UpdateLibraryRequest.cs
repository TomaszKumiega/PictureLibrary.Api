namespace PictureLibrary.Client.Requests
{
    public class UpdateLibraryRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
