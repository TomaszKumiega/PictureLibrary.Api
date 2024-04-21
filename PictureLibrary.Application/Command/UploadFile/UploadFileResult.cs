using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public class UploadFileResult
    {
        public FileCreatedResult? _fileCreatedResult;
        public FileContentAcceptedResult? _fileContentAcceptedResult;

        public UploadFileResult(FileCreatedResult fileCreatedResult)
        {
            _fileCreatedResult = fileCreatedResult;
        }

        public UploadFileResult(FileContentAcceptedResult fileContentAcceptedResult)
        {
            _fileContentAcceptedResult = fileContentAcceptedResult;
        }

        public bool IsUploadFinished { get; set; }
        public object? Value => IsUploadFinished ? _fileCreatedResult : _fileContentAcceptedResult;
    }
}