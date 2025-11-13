using System;
using System.Net.Http.Json;
using System.Text.Json;
using ClientBlazorApp.Services.ServiceInterfaces;
using DTOs.UserDTOs;

namespace ClientBlazorApp.Client.Services.ServiceControllers;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<OutputUserDTO> AddUserAsync(CreateUserDTO request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
        return JsonSerializer.Deserialize<OutputUserDTO>(response, JsonOpts)!;
    }

    public async Task<OutputUserDTO> UpdateUserAsync(int id, UpdateUserDTO request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"users/{id}", request);
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }

        return JsonSerializer.Deserialize<OutputUserDTO>(response, JsonOpts)!;

        
    }

    public async Task<OutputUserDTO> GetSingleUserAsync(int id)
    {
        var dto = await client.GetFromJsonAsync<OutputUserDTO>($"users/{id}", JsonOpts)!;
        if (dto is null)
        {
            throw new Exception("User not found.");
        }
        return dto;
    }

    public async Task<IEnumerable<OutputUserDTO>> GetAllUsersAsync(string? username)
    {
        var url = string.IsNullOrWhiteSpace(username)
        ? "users"
        : $"users?username={Uri.EscapeDataString(username)}";

        var list = await client.GetFromJsonAsync<IEnumerable<OutputUserDTO>>(url, JsonOpts)!;
        return list ?? Enumerable.Empty<OutputUserDTO>();

    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage httpResponse = await client.DeleteAsync($"users{id}");
        string response = await httpResponse.Content.ReadAsStringAsync();
        if (!httpResponse.IsSuccessStatusCode)
        {
            throw new Exception(response);
        }
    }
}

