using System;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class ListCommentView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository)
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
        List<Comment> comments = [.. commentRepository.GetManyAsync().OrderBy(p => p.Id)];
        Console.WriteLine("Showing comments:");
        Console.WriteLine("[");
        
        // show all Comments, with their id and Body.
        foreach (Comment comment in comments)
        {
            Console.WriteLine($"\t({comment.Id}): {comment.Body}");
        }

        Console.WriteLine("]");
        
        // print menu
        const string options = """
                               [comment id]) View Comment by id
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
                // open the "view" for showing a single Comment and user info.
                SingleCommentView singleCommentView = new(postRepository, commentRepository, userRepository, postId);
                await singleCommentView.ShowAsync();
                Console.WriteLine(options);
            }
            else
            {
                Console.WriteLine("Invalid option, please try again.");
            }
        }
    }
}
