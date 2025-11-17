using DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    
    private readonly IUserRepository userRepository;
    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<OutputUserDTO>> Login([FromBody] LoginRequestDTO request)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName);
        if (user == null)
        {
            return Unauthorized("User does not exist, please create a new one");
        }

        if (user.UserName != request.UserName)
        {
            return Unauthorized("Invalid Username or Password");
        }

        if (user.Password != request.Password)
        {
            return Unauthorized("Invalid Username or Password");
        }

        var OutputUserDTO = new OutputUserDTO()
        {
            Id = user.Id,
            UserName = user.UserName,
        };
        
        return Ok(OutputUserDTO);
    }
    
}
