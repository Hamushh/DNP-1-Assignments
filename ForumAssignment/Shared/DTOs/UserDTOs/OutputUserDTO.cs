using System;

namespace DTOs.UserDTOs;

public class OutputUserDTO
{
    public int Id { get; set; }
    public required string UserName { get; set; }

    public OutputUserDTO() { }
    
    public OutputUserDTO(int id, string userName)
    {
        Id = id;
        UserName = userName;
    }
}
