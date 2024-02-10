namespace PictureLibrary.Contracts
{
    public class CreateUploadSessionDto
    {
        public required string FileName { get; set; }
        public required string FileLength { get; set; }
    }
}
