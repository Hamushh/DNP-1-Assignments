using System;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class SingleCommentView(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository, int commentId)
{
    private readonly IPostRepository postRepository = postRepository;
    private readonly ICommentRepository commentRepository = commentRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly int commentId = commentId;

    public async Task ShowAsync()
    {
        Comment comment = await commentRepository.GetSingleAsync(commentId);
        Console.WriteLine($"-----------------------------------------------------");
        Console.WriteLine($"Body: {comment.Body}");
        Console.WriteLine($"-----------------------------------------------------");
        
        // use comment repository to load all comments for this post. The Where() method is used to filter the comments by the post id.
        List<Comment> comments = [.. commentRepository.GetManyAsync().Where(c => c.Id == commentId)];

        foreach (Comment comment1 in comments)
        {
            User user = await userRepository.GetSingleAsync(comment.UserId); // For each comment, load the associated user to get the username.
            Console.WriteLine($"{user.UserName}: {comment.Body}");
        }

        Console.WriteLine();
        const string options = """
                               1) Add comment;
                               <) Back
                               """;
        Console.WriteLine(options);

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

            Console.WriteLine("Not supported");
        }
    }

}
