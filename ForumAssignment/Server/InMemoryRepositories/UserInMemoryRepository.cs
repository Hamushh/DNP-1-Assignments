using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{

    private List<User> users;

    public UserInMemoryRepository()
    {
        SeedDataAsync().GetAwaiter();
    }

    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found");
        }
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userSingleToGet = users.SingleOrDefault(u => u.Id == id);
        if (userSingleToGet is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }
        return Task.FromResult(userSingleToGet);

    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }
    
    private async Task SeedDataAsync()
    {
        User user1 = new()
        {
            Id = 1,
            UserName = "User1",
            Password = "123",
        };
        User user2 = new()
        {
            Id = 2,
            UserName = "User2",
            Password = "abc",
        };
        await AddAsync(user1);
        await AddAsync(user2);

    }

}
