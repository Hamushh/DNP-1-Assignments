using System;

namespace DTOs.UserDTOs;

public class CreateUserDTO
{
    public required string UserName { get; set; }
    public required string Password { get; set; }

}
