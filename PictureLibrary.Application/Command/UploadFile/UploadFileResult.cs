using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.UploadFile;

public class UploadFileResult
{
    private readonly FileCreatedResult? _fileCreatedResult;
    private readonly FileContentAcceptedResult? _fileContentAcceptedResult;

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