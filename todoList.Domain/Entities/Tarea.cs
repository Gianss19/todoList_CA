
namespace todoList.Domain;

public class Tarea
{
    public Guid Id { get; private set; }

    public string Nombre { get; private set; }

    public bool IsCompleted { get; private set; }
    
    public DateTime FechaCreacion { get; private set; }
    
    public DateTime? FechaActualizacion { get; private set; }
    
    public Guid UsuarioId { get; private set; } // FK para el usuario que la creo

    
    
    public Tarea(string nombre, Guid usuario_id)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
        if (nombre.Length < 3)
            throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nombre));
        if(nombre.Length > 100)
            throw new ArgumentException("El nombre no puede tener más de 100 caracteres.", nameof(nombre));
        if(usuario_id == Id)
            throw new ArgumentException("El id de la tarea no puede ser el mismo que el de un usuario.", nameof(usuario_id));

        Id = Guid.NewGuid();
        UsuarioId = usuario_id;
        FechaCreacion = DateTime.UtcNow;
        Nombre = nombre.Trim();
        IsCompleted = false;
    }

    public void Completar()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;
        FechaActualizacion = DateTime.UtcNow;
    }

    public void CambiarNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nuevoNombre));
        
        if (nuevoNombre.Length < 3)
            throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nuevoNombre));

        if (nuevoNombre.Length > 100)
            throw new ArgumentException("El nombre no puede tener más de 100 caracteres.", nameof(nuevoNombre));

        Nombre = nuevoNombre.Trim();
        FechaActualizacion = DateTime.UtcNow;
    }
}