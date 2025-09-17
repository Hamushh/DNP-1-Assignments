using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ManageUsersView
{

    private readonly IUserRepository userRepo;

    public ManageUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("\n=== Edit Users ===");
        Console.WriteLine("1. Update user");
        Console.WriteLine("2. Delete user");
        Console.WriteLine("0. Back");

        Console.WriteLine("\nSelect Option: ");
        var input = Console.ReadLine();

        switch (input)
        {
            case "1":
                UpdateAsync();
                break;

            case "2":
                DeleteAsync();
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Invalid Option");
                break;
        }

    }

    public async Task UpdateAsync()
    {
        Console.Write("\nEnter user ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var user = await userRepo.GetSingleAsync(id);
        if (user is null)
        {
            Console.WriteLine("User is not found");
            return;
        }

        Console.WriteLine($"Updating user {user.Id} (Current name: {user.UserName})");
        Console.Write("New Username (leave empty to keep)");
        var newName = Console.ReadLine();

        Console.Write("New Password (leave empty to keep)");
        var newPswd = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(newName))
        {
            user.UserName = newName;
        }

        if (!string.IsNullOrWhiteSpace(newPswd))
        {
            user.Password = newPswd;
        }

        await userRepo.AddAsync(user);
        Console.WriteLine("User Updated");
    }

    public async Task DeleteAsync()
    {
        Console.Write("\nEnter user ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var existing = await userRepo.GetSingleAsync(id);
        if (existing is null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.Write($"Are you sure you want to delete '{existing.UserName}'? (y/N): ");
        var confirm = Console.ReadLine();

        if (!string.Equals(confirm, "y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        await userRepo.DeleteAsync(id);
        Console.WriteLine("User deleted!");
    }

}
