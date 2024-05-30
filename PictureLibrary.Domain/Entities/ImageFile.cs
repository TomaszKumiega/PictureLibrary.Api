﻿using MongoDB.Bson;

namespace PictureLibrary.Domain.Entities
{
    public class ImageFile : IEntity
    {
        public ObjectId Id { get; set; }
        public ObjectId FileId { get; set; }
        public ObjectId UploadSessionId { get; set; }
        public ObjectId LibraryId { get; set; }
        public IEnumerable<ObjectId>? TagIds { get; set; }
    }
}
