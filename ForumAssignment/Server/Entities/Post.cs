using System;

namespace Entities;

public class Post(int Id, string Title, string Body, int UserId)
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
}
