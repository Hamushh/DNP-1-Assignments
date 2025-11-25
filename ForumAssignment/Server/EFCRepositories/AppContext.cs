using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCRepositories;

public class AppContext : DbContext
{
    public DbSet<Post> posts => Set<Post>();
    public DbSet<Comment> comments => Set<Comment>();
    public DbSet<User> users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}