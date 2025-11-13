using System;
using DTOs.UserDTOs;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository? users;
    public AuthController(IUserRepository users)
    {
        this.users = users;
    }

    [HttpPost]
    public async Task<ActionResult<OutputUserDTO>> Login([FromBody] LoginRequest request)
    {
        var u = await users!.GetByUserNameAsync(request.UserName!);
        if (u is null)
        {
            return Unauthorized("user not found");
        }
        
        if (u.Passsword != request.Password)
        {
            return Unauthorized("Invalid password");
        }

        var dto = new OutputUserDTO
        {
                Id = u.Id,
                UserName = u.UserName!
        };
        return dto;
    }

}
