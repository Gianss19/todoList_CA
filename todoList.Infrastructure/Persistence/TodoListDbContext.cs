using Microsoft.EntityFrameworkCore;
using todoList.Domain;

namespace todoList.Infrastructure.Persistence;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options): base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoListDbContext).Assembly);
    }


public DbSet<Tarea> Tareas => Set<Tarea>();
public DbSet<Usuario> Usuarios => Set<Usuario>();
}

