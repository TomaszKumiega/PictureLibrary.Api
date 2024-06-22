using MongoDB.Bson;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using System.Net.Http.Headers;

namespace PictureLibrary.Infrastructure.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileService _fileService;
        private readonly IPathsProvider _pathsProvider;
        private readonly IMissingRangesParser _missingRangesParser;
        private readonly IMissingRangesService _missingRangesService;
        private readonly IFileMetadataRepository _fileMetadataRepository;
        private readonly IUploadSessionRepository _uploadSessionRepository;

        public FileUploadService(
            IFileService fileService,
            IPathsProvider pathsProvider,
            IMissingRangesParser missingRangesParser,
            IMissingRangesService missingRangesService,
            IFileMetadataRepository fileMetadataRepository,
            IUploadSessionRepository uploadSessionRepository)
        {
            _fileService = fileService;
            _pathsProvider = pathsProvider;
            _missingRangesParser = missingRangesParser;
            _missingRangesService = missingRangesService;
            _fileMetadataRepository = fileMetadataRepository;
            _uploadSessionRepository = uploadSessionRepository;
        }

        public bool ShouldFileBeAppended(MissingRanges missingRanges, ContentRangeHeaderValue range)
        {
            ArgumentNullException.ThrowIfNull(range, nameof(range));
            ArgumentNullException.ThrowIfNull(range.From, nameof(range.From));

            return IsContentRangeMatchingLastRange(missingRanges, range);
        }

        public async Task AppendBytesToFile(UploadSession uploadSession, Stream contentStream)
        {
            ArgumentNullException.ThrowIfNull(uploadSession, nameof(uploadSession));
            ArgumentNullException.ThrowIfNull(contentStream, nameof(contentStream));

            await Task.Run(() => _fileService.AppendFile(uploadSession.FileName, contentStream));
        }

        public async Task InsertBytesToFile(UploadSession uploadSession, Stream contentStream, long startingPosition)
        {
            ArgumentNullException.ThrowIfNull(uploadSession, nameof(uploadSession));
            ArgumentNullException.ThrowIfNull(contentStream, nameof(contentStream));

            await Task.Run(() => _fileService.Insert(uploadSession.FileName, contentStream, startingPosition));
        }

        public async Task UpdateUploadSession(UploadSession uploadSession, MissingRanges missingRanges, ContentRangeHeaderValue range)
        {
            ArgumentNullException.ThrowIfNull(uploadSession, nameof(uploadSession));
            ArgumentNullException.ThrowIfNull(range, nameof(range));
            ArgumentNullException.ThrowIfNull(range.From, nameof(range.From));

            UpdateMissingRanges(uploadSession, missingRanges, range);

            if (IsUploadFinished(uploadSession))
            {
                await _uploadSessionRepository.Delete(uploadSession);
            }
            else
            {
                await _uploadSessionRepository.Update(uploadSession);
            }
        }

        public bool IsUploadFinished(UploadSession uploadSession)
        {
            ArgumentNullException.ThrowIfNull(uploadSession, nameof(uploadSession));

            return string.IsNullOrEmpty(uploadSession.MissingRanges);
        }

        public async Task<FileMetadata> AddFileMetadata(UploadSession uploadSession)
        {
            ArgumentNullException.ThrowIfNull(uploadSession, nameof(uploadSession));

            FileMetadata fileMetadata = new()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = uploadSession.UserId,
                FilePath = Path.Combine(_pathsProvider.GetStoragePath(), uploadSession.FileName),
                FileName = uploadSession.FileName
            };
            
            await _fileMetadataRepository.Add(fileMetadata);

            return fileMetadata;
        }

        private bool IsContentRangeMatchingLastRange(MissingRanges missingRanges, ContentRangeHeaderValue range)
        {
            int rangeIndex = _missingRangesService.GetFirstIndexOfMissingRangeContainingAnotherRange(missingRanges, range);
            return missingRanges.Ranges.Count() == rangeIndex + 1;
        }

        private void UpdateMissingRanges(UploadSession uploadSession, MissingRanges missingRanges, ContentRangeHeaderValue range)
        {
            MissingRanges newMissingRanges = _missingRangesService.RemoveRangeFromMissingRanges(missingRanges, range);

            uploadSession.MissingRanges = _missingRangesParser.ToString(newMissingRanges);
        }
    }
}
