using System;

namespace Entities;

public class User(int Id, string UserName, string Pas)
{

    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    
}