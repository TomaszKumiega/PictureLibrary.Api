namespace PictureLibrary.Domain.Services
{
    public interface IFileService
    {
        Stream OpenFile(string filePath);
        void AppendFile(string fileName, Stream contentStream);
        void Insert(string fileName, Stream contentStream, long position);

        /// <summary>
        /// Changes file name, and returns new path.
        /// </summary>
        /// <param name="filePath">Current file path</param>
        /// <param name="newFileName">New file name</param>
        /// <returns></returns>
        string ChangeFileName(string filePath, string newFileName);

        string GetFileMimeType(string filePath);
    }
}
