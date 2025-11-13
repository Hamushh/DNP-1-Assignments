using System;
using System.Net.Http.Json;
using System.Text.Json;
using ClientBlazorApp.Services.ServiceInterfaces;
using DTOs.PostDTOs;

namespace ClientBlazorApp.Client.Services.ServiceControllers;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<OutputPostDTO> AddPostAsync(CreatePostDTO request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("posts", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<OutputPostDTO>(response, JsonOpts)!;
    }

    public async Task<OutputPostDTO> UpdatePostAsync(int id, UpdatePostDTO request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"posts/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<OutputPostDTO>(response, JsonOpts)!;
    }

    public async Task<OutputPostDTO> GetSinglePostAsync(int id)
    {
        var dto = await client.GetFromJsonAsync<OutputPostDTO>($"posts/{id}", JsonOpts)!;
        if (dto is null)
        {
            throw new Exception("Post not found.");
        }
        return dto;
    }

    public async Task<IEnumerable<OutputPostDTO>> GetAllPostsAsync(string? username)
    {
        var url = string.IsNullOrWhiteSpace(username)
            ? "posts"
            : $"posts?username={Uri.EscapeDataString(username)}";

        var list = await client.GetFromJsonAsync<IEnumerable<OutputPostDTO>>(url, JsonOpts)!;
        return list ?? Enumerable.Empty<OutputPostDTO>();
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"posts/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}

