using System.Net.Http.Headers;
using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.Exceptions;
using PictureLibrary.Client.Requests;
using PictureLibrary.Client.Results;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Results;

namespace PictureLibrary.Client.FileUpload;

internal class ImageFileUpload : IImageFileUpload
{
    private const int FileBatchSize = 1024 * 1024; // 1MB

    public async Task<FileCreatedResult> CreateImageFile(IApiHttpClient apiHttpClient, UploadFileRequest request)
    {
        CreateImageUploadSessionResult createUploadSessionResult = await apiHttpClient.Post<CreateImageUploadSessionResult>($"image/createUploadSession", request.UploadSessionDto);
        string uploadSessionId = createUploadSessionResult.UploadSessionId;
        
        long fileSize = request.Content.Length;
        int numberOfChunks = (int)Math.Ceiling(fileSize / (double)FileBatchSize);

        FileCreatedResult? fileCreatedResult = await UploadFile(apiHttpClient, createUploadSessionResult.UploadSessionId, request.Content, fileSize, numberOfChunks);

        if (fileCreatedResult is null)
        {
            throw new InvalidResponseException();
        }

        return fileCreatedResult;
    }

    private async Task<FileCreatedResult?> UploadFile(IApiHttpClient apiHttpClient, string uploadSessionId, Stream content, long fileSize, int numberOfChunks)
    {
        int bytesRead;
        int startIndex = 0;
        string? expectedRanges = null;
        byte[] buffer = new byte[fileSize];
        Memory<byte> fileContentMemory = new Memory<byte>(buffer);
        FileUploadResult? fileUploadResult = null;
        
        for (int i = 0; i < numberOfChunks; i++)
        {
            startIndex = i * FileBatchSize;
            content.Seek(startIndex, SeekOrigin.Begin);
            
            bytesRead = await content.ReadAsync(fileContentMemory);

            if (bytesRead == 0) // end of file
            {
                break;
            }

            var rangeHeaderValue = GetRangeHeaderValue(startIndex, startIndex + bytesRead);
            fileUploadResult = await apiHttpClient.UploadFile($"image/upload?uploadSessionId={uploadSessionId}", fileContentMemory, rangeHeaderValue);
            
            if (fileUploadResult is { UploadFinished: true, FileCreatedResult: not null })
            {
                return fileUploadResult.FileCreatedResult; // upload finished
            }
            
            expectedRanges = fileUploadResult?.FileContentAcceptedResult?.ExpectedRanges;
            if (fileUploadResult?.FileCreatedResult is null && string.IsNullOrEmpty(expectedRanges))
            {
                throw new InvalidResponseException(); // no expected ranges but upload is not finished
            }
        }

        if (fileUploadResult is null)
        {
            throw new InvalidResponseException();
        }
        
        // reached eof but still no upload finished result from API
        fileUploadResult = await HandleEndOfFile(apiHttpClient, uploadSessionId, content, fileUploadResult);
        
        return fileUploadResult.FileCreatedResult;
    }

    private async Task<FileUploadResult> HandleEndOfFile(IApiHttpClient apiHttpClient, string uploadSessionId, Stream content, FileUploadResult fileUploadResult)
    {
        for (int i = 0; i < 3; i++) // 3 retries for uploading missing ranges
        {
            string? expectedRanges = fileUploadResult.FileContentAcceptedResult?.ExpectedRanges;
            var missingRanges = GetMissingRanges(expectedRanges);
            fileUploadResult = await HandleMissingRanges(apiHttpClient, uploadSessionId, content, missingRanges);

            if (fileUploadResult.UploadFinished)
            {
                return fileUploadResult;
            }

            if (string.IsNullOrEmpty(fileUploadResult?.FileContentAcceptedResult?.ExpectedRanges))
            {
                throw new InvalidResponseException();
            }

            if (expectedRanges == fileUploadResult.FileContentAcceptedResult?.ExpectedRanges)
            {
                throw new InvalidResponseException(); // there was an error in uploading last batch
            }
        }

        return fileUploadResult;
    }

    private static IEnumerable<(long? From, long? To)> GetMissingRanges(string? missingRanges)
    {
        if (string.IsNullOrEmpty(missingRanges))
        {
            return Enumerable.Empty<(long? From, long? To)>();
        }
        
        RangeHeaderValue rangeHeaderValue = RangeHeaderValue.Parse(missingRanges);

        return rangeHeaderValue.Ranges.Select(x => (x.From, x.To));
    }

    private async Task<FileUploadResult> HandleMissingRanges(IApiHttpClient apiHttpClient, string uploadSessionId, Stream content, IEnumerable<(long? From, long? To)> missingRanges)
    {
        long startIndex;
        long count = 0;
        
        int bytesRead;
        FileUploadResult? fileUploadResult = null;

        foreach (var missingRange in missingRanges)
        {
            startIndex = missingRange.From ?? 0;
            count += !missingRange.To.HasValue
                ? content.Length - startIndex
                : missingRange.To.Value - startIndex;

            byte[] buffer = new byte[count];
            Memory<byte> fileContentMemory = new Memory<byte>(buffer);
            
            content.Seek(startIndex, SeekOrigin.Begin);
            
            bytesRead = await content.ReadAsync(fileContentMemory);

            if (bytesRead == 0)
            {
                continue;
            }

            var rangeHeaderValue = GetRangeHeaderValue(startIndex, startIndex + bytesRead);
            fileUploadResult = await apiHttpClient.UploadFile($"image/upload?uploadSessionId={uploadSessionId}", fileContentMemory, rangeHeaderValue);
        }

        return fileUploadResult ?? throw new InvalidResponseException();
    }

    private RangeHeaderValue GetRangeHeaderValue(long from, long to)
    {
        return new RangeHeaderValue(from, to);
    }
}