using System;

namespace Entities;

public class User
{

    public User(int Id, string UserName, string Password)
    {
        this.Id = Id;
        this.UserName = UserName;
        this.Password = Password;
    }
    
    public User() {}
    
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    
}