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
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<Post> AddAsync(Post post)

    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        int maxId = (int)(posts.Count > 0 ? posts.Max(p => p.Id) : 1);
        post.Id = maxId + 1;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public  async Task Delete(int id)
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

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Post> GetManyAsync()
    {
        string postsAsJson =  File.ReadAllTextAsync(filePath).Result;
        List < Post > posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    } 

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List < Post > posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.SingleOrDefault(p => p.Id == id);
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
