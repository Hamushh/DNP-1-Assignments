using System;
using System.Net.Http.Json;
using System.Text.Json;
using ClientBlazorApp.Services.ServiceInterfaces;
using DTOs.CommentDTOs;

namespace ClientBlazorApp.Client.Services.ServiceControllers;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<OutputCommentDTO> AddCommentAsync(CreateCommentDTO request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("comments", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<OutputCommentDTO>(response, JsonOpts)!;
    }

    public async Task<OutputCommentDTO> UpdateCommentAsync(int id, UpdateCommentDTO request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"comments/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<OutputCommentDTO>(response, JsonOpts)!;
    }

    public async Task<OutputCommentDTO> GetSingleCommentAsync(int id)
    {
        var dto = await client.GetFromJsonAsync<OutputCommentDTO>($"comments/{id}", JsonOpts)!;
        if (dto is null)
        {
            throw new Exception("Comment not found.");
        }
        return dto;
    }

    public async Task<IEnumerable<OutputCommentDTO>> GetAllCommentsAsync(string? username)
    {
        var url = string.IsNullOrWhiteSpace(username)
            ? "comments"
            : $"comments?username={Uri.EscapeDataString(username)}";

        var list = await client.GetFromJsonAsync<IEnumerable<OutputCommentDTO>>(url, JsonOpts)!;
        return list ?? Enumerable.Empty<OutputCommentDTO>();
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"comments/{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}

