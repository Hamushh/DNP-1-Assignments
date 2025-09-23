using CLI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting the CLI app...");
IUserRepository userRepository = new UserFileRepository();
IPostRepository postRepository = new PostFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

CliApp app = new(userRepository, postRepository, commentRepository);
await app.StartAsync();