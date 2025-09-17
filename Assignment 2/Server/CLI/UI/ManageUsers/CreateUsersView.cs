using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUsersView
{
    private readonly IUserRepository userRepo;

    public CreateUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("\n=== Create New User ===");

        Console.WriteLine("Enter Username: ");
        string name = Console.ReadLine();

        Console.WriteLine("Enter Password: ");
        string password = Console.ReadLine();

        var user = new User() { UserName = name, Password = password };

        await userRepo.AddAsync(user);

        Console.WriteLine($"User '{name}' created.");
    }

}
