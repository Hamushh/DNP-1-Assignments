using System;

namespace Entities;

public class Comment
{

    public Comment(int Id, int PostId, int UserId, string Body)
    {
        this.Id = Id;
        this.PostId = PostId;
        this.UserId = UserId;
        this.Body = Body;
    }
    
    public Comment() {}
    
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }

}
