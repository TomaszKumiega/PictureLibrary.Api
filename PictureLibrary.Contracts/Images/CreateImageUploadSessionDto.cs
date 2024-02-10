namespace PictureLibrary.Contracts
{
    public class CreateImageUploadSessionDto
    {
        public required string FileName { get; set; }
        public required int FileLength { get; set; }
    }
}
