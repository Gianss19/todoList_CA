using System.Data;
using System.Text.Json.Serialization;

namespace todoList.Domain;

public class Tarea
{
    public Guid Id { get; private set; }

    public string Nombre { get; private set; }

    public bool IsCompleted { get; private set; }

    [JsonConstructor]
    public Tarea(Guid id, string nombre, bool isCompleted)
    {
        Id = id;
        Nombre = nombre;
        IsCompleted = isCompleted;
    }
    
    public Tarea(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));

        nombre = nombre.Trim();

        if (nombre.Length < 3)
            throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nombre));

        Id = Guid.NewGuid();
        Nombre = nombre;
        IsCompleted = false;
    }

    public void Completar()
    {
        if (IsCompleted)
            return;

        IsCompleted = true;
    }

    public void CambiarNombre(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nuevoNombre));

        nuevoNombre = nuevoNombre.Trim();

        if (nuevoNombre.Length < 3)
            throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nuevoNombre));

        Nombre = nuevoNombre;
    }
}