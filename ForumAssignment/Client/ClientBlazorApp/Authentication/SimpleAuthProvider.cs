using System.Security.Claims;
using System.Text.Json;
using DTOs.UserDTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientBlazorApp.Authentication;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private ClaimsPrincipal currentClaimsPrincipal;
    
    public SimpleAuthProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "api/auth/login", new LoginRequestDTO()
            {
                UserName = userName,
                Password = password
            });
        
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }
        
        OutputUserDTO userDto = JsonSerializer.Deserialize<OutputUserDTO>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;

        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim("Id", userDto.Id.ToString()),
        };
        
        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        currentClaimsPrincipal = new ClaimsPrincipal(identity);
        
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
    
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = currentClaimsPrincipal ?? new ClaimsPrincipal(new ClaimsIdentity());
        return Task.FromResult(new AuthenticationState(principal));
    }

    public void Logout()
    {
        currentClaimsPrincipal = new();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentClaimsPrincipal)));
    }
}