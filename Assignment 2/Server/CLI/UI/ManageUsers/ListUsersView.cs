using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView(IUserRepository userRepository)
{
    private readonly IUserRepository userRepository = userRepository;

    public void Show()
    {
        Console.WriteLine();
        ViewUsers();
    }

    private void ViewUsers()
    {
        IEnumerable<User> many = userRepository.GetManyAsync();
        List<User> users = [.. many.OrderBy(u => u.Id)];

        Console.WriteLine("Users:");
        Console.WriteLine("[");
        foreach (User user in users)
        {
            Console.WriteLine($"\tID: {user.Id}, Name: {user.UserName}");
        }
        Console.WriteLine("]");
        Console.WriteLine();
    }
}
