﻿namespace PictureLibrary.Application.Dto
{
    public class LibraryDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
