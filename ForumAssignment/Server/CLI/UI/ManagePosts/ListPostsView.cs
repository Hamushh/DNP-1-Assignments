using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
{
    private readonly IPostRepository postRepository = postRepository;
    private readonly ICommentRepository commentRepository = commentRepository;
    private readonly IUserRepository userRepository = userRepository;

    public Task ShowAsync()
    {
        Console.WriteLine();
        return ViewPostsAsync();
    }

    private async Task ViewPostsAsync()
    {
        // This chain of methods will be introduced later in the course.
        // I'm just so used to it, I find it har do do normal loops and stuff.
        // I get all posts, order them by id.
        List<Post> posts = [.. postRepository.GetManyAsync().OrderBy(p => p.Id)];
        Console.WriteLine("Showing posts:");
        Console.WriteLine("[");
        
        // show all posts, with their id and title.
        foreach (Post post in posts)
        {
            Console.WriteLine($"\t({post.Id}): {post.Title}");
        }

        Console.WriteLine("]");
        
        // print menu
        const string options = """
                               [post id]) View post by id
                               <) Back
                               """;
        Console.WriteLine(options);
        
        // read menu selection
        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please select a valid option.");
                continue;
            }

            if ("<".Equals(input))
            {
                return;
            }

            if (int.TryParse(input, out int postId))
            {
                // open the "view" for showing a single post, with comments and user info.
                SinglePostView singlePostView = new(postRepository, commentRepository, userRepository, postId);
                await singlePostView.ShowAsync();
                Console.WriteLine(options);
            }
            else
            {
                Console.WriteLine("Invalid option, please try again.");
            }
        }
    }
}
