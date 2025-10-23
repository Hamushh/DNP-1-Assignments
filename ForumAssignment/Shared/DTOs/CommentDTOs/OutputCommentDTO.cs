using System;

namespace DTOs.CommentDTOs;

public class OutputCommentDTO
{
    public required int Id { get; set; }
    public required string Body { get; set; }
    public required int PostId { get; set; }
    public required int UserId { get; set; }

}