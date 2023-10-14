using Microsoft.EntityFrameworkCore;
using ToDoGrpc.Models;

namespace ToDoGrpc.Data;

public class AppDbContext : DbContext
{
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDoItem>()
            .Property(x => x.ToDtoStatus).HasMaxLength(255);

        modelBuilder.Entity<ToDoItem>()
            .Property(x => x.Title).HasMaxLength(255);

        modelBuilder.Entity<ToDoItem>()
            .Property(x => x.Desc).HasMaxLength(255);

        modelBuilder.Entity<ToDoItem>().HasData(
            new ToDoItem { Id = 1, Title = "First ToDo", Desc = "This is the first ToDo item" },
            new ToDoItem { Id = 2, Title = "Second ToDo", Desc = "This is the second ToDo item" },
            new ToDoItem { Id = 3, Title = "Third ToDo", Desc = "This is the third ToDo item" }
        );
    }
}