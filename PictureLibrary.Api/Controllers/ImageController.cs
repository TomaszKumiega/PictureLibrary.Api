﻿using System.Net.Http.Headers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.CreateImageUploadSession;
using PictureLibrary.Application.Command.DeleteImageFile;
using PictureLibrary.Application.Command.UpdateImageFile;
using PictureLibrary.Application.Command.UploadFile;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Query.GetAllImageFiles;
using PictureLibrary.Application.Query.GetImageFileContent;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers;

[Route("image")]
[ApiController]
public class ImageController(IMediator mediator) : ControllerBase(mediator)
{
    [HttpPost("createUploadSession")]
    public async Task<IActionResult> CreateUploadSession(
        [FromBody] CreateImageUploadSessionDto? createUploadSessionDto)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (createUploadSessionDto == null)
        {
            return BadRequest();
        }
        
        var command = new CreateImageUploadSessionCommand(userId, createUploadSessionDto);

        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadFile(
        [FromQuery] string uploadSessionId, 
        [FromHeader(Name = "Content-Range")] string contentRangeString)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        ContentRangeHeaderValue? contentRange = GetContentRange(contentRangeString);

        if (contentRange == null)
        {
            return BadRequest();
        }

        var command = new UploadFileCommand(userId, uploadSessionId, contentRange, Request.Body);

        var result = await Mediator.Send(command);

        return result.IsUploadFinished
            ? Created("/image/get", result.Value)
            : Accepted(result.Value);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] string imageFileId)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(imageFileId))
        {
            return BadRequest();
        }

        var command = new DeleteImageFileCommand(userId, imageFileId);

        await Mediator.Send(command);

        return Ok();
    }

    [HttpPatch("update")]
    public async Task<IActionResult> Update(
        [FromQuery] string imageFileId,
        [FromBody] UpdateImageFileDto? updateImageFileDto)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (updateImageFileDto == null || string.IsNullOrEmpty(imageFileId))
        {
            return BadRequest();
        }
        
        var command = new UpdateImageFileCommand(userId, imageFileId, updateImageFileDto);

        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] string libraryId)
    {
        string? userId = GetUserId();

        if (userId == null) 
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(libraryId))
        {
            return BadRequest();
        }

        var command = new GetAllImageFilesQuery(userId, libraryId);

        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("content")]
    public async Task<IActionResult> GetContent([FromQuery] string imageFileId)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(imageFileId))
        {
            return BadRequest();
        }
        
        var command = new GetImageFileContentQuery(userId, imageFileId);
            
        var result = await Mediator.Send(command);

        return File(result.Content, result.ContentType);
    }
}