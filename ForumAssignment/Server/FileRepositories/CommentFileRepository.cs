using System;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository

{
    private readonly string filePath = "comments.json";
    private List<Comment> comments;

    public CommentFileRepository()

    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<Comment> AddAsync(Comment comment)

    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        int maxId = (int)(comments.Count > 0 ? comments.Max(c => c.Id) : 1);
        comment.Id = maxId + 1;
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        comments.Remove(commentToRemove);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }
    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }
        comments.Remove(existingComment);
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        Comment? commentGetSingle = comments.SingleOrDefault(c => c.Id == id);
        return commentGetSingle!;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsByPostIdAsync(int postId)
    {
        
        var allComments = GetManyAsync().ToList();
    
        // Filter by post ID
        return allComments.Where(c => c.PostId == postId);
    }

}
