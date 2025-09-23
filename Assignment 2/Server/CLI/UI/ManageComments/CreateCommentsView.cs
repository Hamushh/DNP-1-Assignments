using System;
using System.Reflection;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView(ICommentRepository commentRepository)
{
    private readonly ICommentRepository commentRepository = commentRepository;

    public Task ShowAsync()
    {
        Console.WriteLine();
        return CreateCommentAsync();
    }

    private Task CreateCommentAsync()
    {
        while (true)
        {
            Console.WriteLine("You are creating a comment.");
            Console.WriteLine("Please insert comment content:");
            string? content = "";

            while (string.IsNullOrEmpty(content))
            {
                content = Console.ReadLine();
                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("Content cannot be empty.");
                    continue;
                }

                if ("<".Equals(content))
                {
                    Console.WriteLine("Post creation cancelled.");
                    return Task.CompletedTask;
                }
            }

            Console.WriteLine("Please insert the ID of the user that created the comment:");

            int userId;

            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("User ID cannot be empty.");
                    continue;
                }

                if ("<".Equals(input))
                {
                    Console.WriteLine("User creation cancelled.");
                    return Task.CompletedTask;
                }

                if (int.TryParse(input, out userId))
                {
                    
                    break;
                }
                else
                {
                    Console.WriteLine("Could not parse the ID, please try again.");
                }
            }

            int postId;

            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Post ID cannot be empty.");
                    continue;
                }

                if ("<".Equals(input))
                {
                    Console.WriteLine("Comment creation cancelled.");
                    return Task.CompletedTask;
                }

                if (int.TryParse(input, out postId))
                {
                    
                    break;
                }
                else
                {
                    Console.WriteLine("Could not parse the Post ID, please try again.");
                }
            }

            

            Console.WriteLine("You are about to create a comment.");
            Console.WriteLine("Do you want to proceed? (y/n)");
            string? confirmation;

            while (true)
            {
                confirmation = Console.ReadLine();
                if (string.IsNullOrEmpty(confirmation))
                {
                    Console.WriteLine("Please select an option.\n\n");
                    continue;
                }

                confirmation = confirmation.ToLower();
                if (confirmation != "y" && confirmation != "n")
                {
                    Console.WriteLine("Please select a valid option.\n\n");
                    continue;
                }

                break;
            }

            switch (confirmation.ToLower())
            {
                case "y": return AddCommentAsync(content, userId, postId);
                case "n":
                    {
                        Console.WriteLine("User creation cancelled.");
                        return Task.CompletedTask;
                    }
                default:
                    Console.WriteLine("Invalid option, please try again.\n\n");
                    break;
            }
        }
    }

    private async Task AddCommentAsync(string content, int userId, int postId)
    {
        Comment comment = new Comment() { Body = content, UserId = userId, PostId = postId };
        Comment added = await commentRepository.AddAsync(comment);
        Console.WriteLine("Comment created successfully, with Id: " + added.Id);
    }

}
