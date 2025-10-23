using System;

namespace DTOs.PostDTOs;

public class CreatePostDTO
{
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required int UserId { get; set; }
}
