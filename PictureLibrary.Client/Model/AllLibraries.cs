namespace PictureLibrary.Client.Model
{
    public class AllLibraries
    {
        public required IEnumerable<Library> Libraries { get; set; }
    }
}