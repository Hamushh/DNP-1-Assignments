using CLI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting the CLI app...");

IUserRepository userRepository = new UserInMemoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();
ICommentRepository commentRepository = new CommentInMemoryRepository();

CliApp app = new(userRepository, postRepository, commentRepository);

await app.StartAsync();