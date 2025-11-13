using System;

namespace DTOs.CommentDTOs;

public class UpdateCommentDTO
{
    public required string Body { get; set; }
    public required int PostId { get; set; }
    public required int UserId { get; set; }
}
