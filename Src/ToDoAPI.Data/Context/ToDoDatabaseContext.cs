using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Context;

public partial class ToDoDatabaseContext : DbContext
{
    public ToDoDatabaseContext()
    {
    }

    public ToDoDatabaseContext(DbContextOptions<ToDoDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Todo> Todos { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ToDoDatabaseContext).Assembly);
}