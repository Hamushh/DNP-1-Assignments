using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{

    private List<Comment> comments;

    public CommentInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter();
    }

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }
        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? commentSingleToGet = comments.SingleOrDefault(c => c.Id == id);
        if (commentSingleToGet is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        comments.Remove(commentSingleToGet);
        return Task.FromResult(commentSingleToGet);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return comments.AsQueryable();
    }

    private async Task SeedDataAsync()
    {
        Comment comment1 = new()
        {
            Id = 1,
            Body = "comment1",
            PostId = 1,
            UserId = 1,
        };
        Comment comment2 = new()
        {
            Id = 2,
            Body = "comment2",
            PostId = 2,
            UserId = 2,
        };
        Comment comment3 = new()
        {
            Id = 3,
            Body = "comment3",
            PostId = 2,
            UserId = 2,
        };
        await AddAsync(comment1);
        await AddAsync(comment2);
        await AddAsync(comment3);
    }
}
