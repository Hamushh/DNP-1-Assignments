using CLI.UI.ManageUsers;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CLIApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CLIApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine("\n=== Main Menu ===");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("3. Manage Users");
            Console.WriteLine("0. Exit");

            Console.WriteLine("Select Option: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    var createUsersView = new CreateUsersView(userRepository);
                    await createUsersView.ShowAsync();
                    break;

                case "2":
                    var listUsersView = new ListUsersView(userRepository);
                    await listUsersView.ShowAsync();
                    break;

                case "3":
                    var manageUsersView = new ManageUsersView(userRepository);
                    await manageUsersView.ShowAsync();
                    break;

                case "0":
                    Console.WriteLine("Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid Option. Please try again.");
                    break;
            }
        }
    }

}
