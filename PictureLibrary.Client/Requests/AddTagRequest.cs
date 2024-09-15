namespace PictureLibrary.Client.Requests
{
    public class AddTagRequest
    {
        public required string Name { get; set; }
        public required string ColorHex { get; set; }
    }
}
