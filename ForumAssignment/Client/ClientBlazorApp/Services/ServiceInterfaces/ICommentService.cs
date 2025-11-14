using System;
using DTOs.CommentDTOs;

namespace ClientBlazorApp.Services.ServiceInterfaces;

public interface ICommentService
{
    public Task<OutputCommentDTO> AddCommentAsync(CreateCommentDTO request);

    public Task<OutputCommentDTO> UpdateCommentAsync(int id, UpdateCommentDTO request);

    public Task<OutputCommentDTO> GetSingleCommentAsync(int id);

    public Task<IEnumerable<OutputCommentDTO>> GetAllCommentsAsync(string? username);
    
    public Task<IEnumerable<OutputCommentDTO>> GetCommentsByPostIdAsync(int postId);
    
    public Task DeleteCommentAsync(int id);
}
