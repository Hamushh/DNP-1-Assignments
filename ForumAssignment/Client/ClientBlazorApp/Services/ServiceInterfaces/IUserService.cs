using System;
using DTOs.UserDTOs;

namespace ClientBlazorApp.Services.ServiceInterfaces;

public interface IUserService
{
    public Task<OutputUserDTO> AddUserAsync(CreateUserDTO request);

    public Task<OutputUserDTO> UpdateUserAsync(int id, UpdateUserDTO request);

    public Task<OutputUserDTO> GetSingleUserAsync(int id);

    public Task<IEnumerable<OutputUserDTO>> GetAllUsersAsync(string? username);

    public Task DeleteUserAsync(int id);
}
