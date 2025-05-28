namespace PictureLibrary.Contracts.Results;

public record GetAllLibrariesResult(IEnumerable<LibraryDto> Libraries);