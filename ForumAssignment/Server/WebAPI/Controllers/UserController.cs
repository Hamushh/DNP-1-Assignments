using System;
using Entities;
using DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController :  ControllerBase
{
    private readonly IUserRepository userRepository;

    private async Task<bool> VerifyIfUserNameIsAvailableAsync(string username)
    {
        var users = userRepository.GetManyAsync();

        bool exists = users.Any(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        return !exists;
    }

    public UserController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<OutputUserDTO>> CreateUser([FromBody] CreateUserDTO request)
    {
        try
        {
            if (!await VerifyIfUserNameIsAvailableAsync(request.UserName))
            {
                return Conflict($"Username '{request.UserName}' is already taken.");
            }

            User user = new(0, request.UserName, request.Password);
            
            User created = await userRepository.AddAsync(user);

            return Created($"/users/{created.Id}", new OutputUserDTO()
            {
                Id = created.Id,
                UserName = created.UserName
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }


    [HttpGet]
    public ActionResult<IEnumerable<OutputUserDTO>> GetAllUsers([FromQuery] string? username)
    {
        var users = userRepository.GetManyAsync();
        List<OutputUserDTO> dtoUsers = new();
        if (!string.IsNullOrWhiteSpace(username))
        {
            users = users.Where(u => u.UserName.Contains(username, StringComparison.OrdinalIgnoreCase));
        }

        foreach(var user in users)
        {
            dtoUsers.Add(new OutputUserDTO()
            {
                Id = user.Id,
                UserName = user.UserName
            });
        }

        return Ok(dtoUsers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OutputUserDTO>> GetUser(int id)
    {
        try
        {
            var user = await userRepository.GetSingleAsync(id);
            return Ok(new OutputUserDTO()
            {
                Id = user.Id,
                UserName = user.UserName
            });
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] CreateUserDTO request)
    {
        try
        {
            var existing = await userRepository.GetSingleAsync(id);
            existing.UserName = request.UserName;
            existing.Password = request.Password;
            await userRepository.UpdateAsync(existing);
            return NoContent();
        }
        catch (Exception e)
        { 
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}
