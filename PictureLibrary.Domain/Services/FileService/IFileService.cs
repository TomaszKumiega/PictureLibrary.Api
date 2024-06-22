using System.Drawing;

namespace PictureLibrary.Domain.Services
{
    public interface IFileService
    {
        void AppendFile(string fileName, Stream contentStream);
        void Insert(string fileName, Stream contentStream, long position);
        
        /// <summary>
        /// Changes file name, and returns new path.
        /// </summary>
        /// <param name="filePath">Current file path</param>
        /// <param name="newFileName">New file name</param>
        /// <returns></returns>
        string ChangeFileName(string filePath, string newFileName);

        Icon GetFileIcon(string filePath);
    }
}
