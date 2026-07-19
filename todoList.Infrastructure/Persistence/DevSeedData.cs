using todoList.Domain;

namespace todoList.Infrastructure.Persistence;

public static class DevSeedData
{
    public static void Seed(TodoListDbContext context)
    {
        if (context.Usuarios.Any())
            return;

        var adminId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var admin = new Usuario("Admin", "admin@dev.local", BCrypt.Net.BCrypt.HashPassword("Admin123!"));

        var usuario = new Usuario("Usuario Dev", "user@dev.local", BCrypt.Net.BCrypt.HashPassword("User123!"));

        context.Usuarios.AddRange(admin, usuario);
        context.SaveChanges();

        context.Tareas.AddRange(
            new Tarea("Tarea de ejemplo 1", admin.Id),
            new Tarea("Tarea de ejemplo 2", admin.Id),
            new Tarea("Tarea del usuario", usuario.Id)
        );
        context.SaveChanges();
    }
}
