using System;
using DTOs.PostDTOs;

namespace ClientBlazorApp.Services.ServiceInterfaces;

public interface IPostService
{
    public Task<OutputPostDTO> AddPostAsync(CreatePostDTO request);

    public Task<OutputPostDTO> UpdatePostAsync(int id, UpdatePostDTO request);

    public Task<OutputPostDTO> GetSinglePostAsync(int id);

    public Task<IEnumerable<OutputPostDTO>> GetAllPostsAsync(string? username);

    public Task DeletePostAsync(int id);
}
