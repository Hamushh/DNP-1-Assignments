using System;
using DTOs.CommentDTOs;
using Entities;
using FileRepositories;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepo;

    public CommentController(ICommentRepository commentRepo)
    {
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<OutputCommentDTO>> CreateComment([FromBody] CreateCommentDTO request)
    {
        try
        {
            var comment = new Comment(0, request.UserId, request.PostId, request.Body);

            var created = await commentRepo.AddAsync(comment);

            return Created($"/Users/Comments/{created.Id}", new OutputCommentDTO
            {
                Id = created.Id,
                UserId = created.UserId,
                PostId = created.PostId,
                Body = created.Body
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OutputCommentDTO>> GetSingleComment([FromRoute] int id)
    {
        try
        {
            var comment = await commentRepo.GetSingleAsync(id);

            return Ok(new OutputCommentDTO()
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Body = comment.Body
            });
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }

    [HttpGet]
    public ActionResult<List<OutputCommentDTO>> GetAllComments([FromQuery] int? postId)
    {
        try
        {
            var comments = commentRepo.GetManyAsync();
            List<OutputCommentDTO> dtoComments = new();

            if (postId != null)
            {
                comments = comments.Where(c => c.PostId == postId);
            }

            foreach (var comment in comments)
            {
                dtoComments.Add(new OutputCommentDTO()
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    Body = comment.Body
                });
            }

            return Ok(dtoComments);
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OutputCommentDTO>> UpdateComment([FromRoute] int id, [FromBody] CreateCommentDTO request)
    {
        try
        {
            var comment = await commentRepo.GetSingleAsync(id);
            comment.Body = request.Body;
            await commentRepo.UpdateAsync(comment);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int id)
    {
        try
        {
            await commentRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Comment with ID {id} not found.");
        }
    }
}
