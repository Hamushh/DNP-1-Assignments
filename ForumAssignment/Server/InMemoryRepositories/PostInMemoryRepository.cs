using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{

    private List<Post> posts;

    public PostInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter();
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
        }
        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? postSingleToGet = posts.SingleOrDefault(p => p.Id == id);
        if (postSingleToGet is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        return Task.FromResult(postSingleToGet);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }

    private async Task SeedDataAsync()
    {
        Post post1 = new()
        {
            Id = 1,
            Title = "post1title",
            Body = "post1",
            UserId = 1,
        };
        Post post2 = new()
        {
            Id = 2,
            Title = "post2title",
            Body = "post2",
            UserId = 2,
        };
        Post post3 = new()
        {
            Id = 3,
            Title = "post3title",
            Body = "post3",
            UserId = 1,
        };
        await AddAsync(post1);
        await AddAsync(post2);
        await AddAsync(post3);
    }

}
