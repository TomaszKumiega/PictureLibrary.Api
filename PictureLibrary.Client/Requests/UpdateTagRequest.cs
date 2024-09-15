namespace PictureLibrary.Client.Requests
{
    public class UpdateTagRequest
    {
        public required string Name { get; set; }
        public required string ColorHex { get; set; }
    }
}
