using System;

namespace Entities;

public class Comment(int Id, int PostId, int UserId, string Body)
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }

}
