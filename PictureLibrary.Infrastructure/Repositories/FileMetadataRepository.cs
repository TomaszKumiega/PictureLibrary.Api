using MongoDB.Driver;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Infrastructure.Repositories;

public class FileMetadataRepository(IAppSettings appSettings, IMongoClient mongoClient) 
    : Repository<FileMetadata>(appSettings, mongoClient), IFileMetadataRepository
{
    protected override string CollectionName => "FilesMetadata";
}