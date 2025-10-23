using System;
using Entities;
using DTOs.PostDTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;
[ApiController]
[Route("[controller]")]

public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    
    public PostsController(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<OutputPostDTO>> AddPost([FromBody] CreatePostDTO request)
    {
        try
        {
            Post post = new(0, request.Body, request.Title, request.UserId);
            var created = await postRepository.AddAsync(post);
            
            return Created($"/posts/{created.Id}", new OutputPostDTO 
            { 
                Id = created.Id, 
                UserId = created.UserId,
                Title = created.Title,
                Body = created.Body
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    public ActionResult<List<OutputPostDTO>> GetAllPosts([FromQuery] string? title)
    {
        try
        {
            var posts = postRepository.GetManyAsync();
            List<OutputPostDTO> dtoPosts = new();
        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var post in posts)
        {
            dtoPosts.Add(new OutputPostDTO()
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Body = post.Body
            });
        }
            return Ok(dtoPosts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OutputPostDTO>> GetPost([FromRoute] int id)
    {
        try
        {
            var post = await postRepository.GetSingleAsync(id);

            return Ok(new OutputPostDTO()
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Body = post.Body
            });
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");  
        } 
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] CreatePostDTO request)
    {
        try
        {
            var post = await postRepository.GetSingleAsync(id);
            
            post.UserId = request.UserId;
            post.Title = request.Title;
            post.Body = request.Body;

            await postRepository.UpdateAsync(post);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePost([FromRoute] int id)
    {
        try
        {
            await postRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound($"Post with ID {id} not found.");
        }
    }
}
