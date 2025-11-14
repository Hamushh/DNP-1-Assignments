using System;
using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository

{
    private readonly string filePath = "posts.json";
    private List<Post> posts;

    public PostFileRepository()
    {
        filePath = Path.Combine(AppContext.BaseDirectory, "posts.json");

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    
    public async Task<Post> AddAsync(Post post)

    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post>? posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Post>();
        
        int newId = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
        post.Id = newId;

        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(filePath, postsAsJson);

        return post;
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List < Post > posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }
        posts.Remove(postToRemove);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public IQueryable<Post> GetManyAsync()
    {
        string postsAsJson =  File.ReadAllTextAsync(filePath).GetAwaiter().GetResult();
        List < Post > posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    } 

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List < Post > posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
            throw new InvalidOperationException($"Post with ID {id} not found.");

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List < Post > posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        int index = posts.FindIndex(c => c.Id == post.Id);
        if (index == -1)
        {
            throw new InvalidOperationException($"Post with Id {post.Id} not found");
        }
        posts[index] = post;
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    } 
}
