using PictureLibrary.Domain.Entities;
using System.Net.Http.Headers;

namespace PictureLibrary.Domain.Services
{
    public interface IFileUploadService
    {
        bool ShouldFileBeAppended(MissingRanges missingRanges, ContentRangeHeaderValue range);
        Task AppendBytesToFile(UploadSession uploadSession, Stream contentStream);
        Task InsertBytesToFile(UploadSession uploadSession, Stream contentStream, long startingPosition);
        Task UpdateUploadSession(UploadSession uploadSession, MissingRanges missingRanges, ContentRangeHeaderValue range);
        bool IsUploadFinished(UploadSession uploadSession);
        Task<FileMetadata> AddFileMetadata(UploadSession uploadSession);
    }
}
