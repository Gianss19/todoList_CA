
using System.Text.RegularExpressions;

namespace todoList.Domain;

public class Usuario
{
 public Guid Id {get; private set;}
 public string Nombre{get; private set;}
 public string Correo{get; private set;}
 public string PasswordHash{get; private set;}
 public bool Activo {get; private set;} = true;
 public DateTime FechaCreacion {get; private set;}
 public DateTime? FechaActualizacion{get; private set;}
 public ICollection<Tarea> Tareas{get; private set;}

private Usuario()
    {
        
    }
public Usuario(string nombre, string correo, string passwordHash)
    {

        if(string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nombre));
        if(nombre.Length < 3)
            throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nombre));
        if (nombre.Length > 100)
            throw new ArgumentException("El nombre no puede tener más de 100 caracteres.", nameof(nombre));

        if (string.IsNullOrWhiteSpace(correo) || !Regex.IsMatch(correo, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", RegexOptions.IgnoreCase))
            throw new ArgumentException("El correo no es válido.", nameof(correo));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("El hash no puede estar vacío.", nameof(passwordHash));
        if(passwordHash.Length != 60)
            throw new ArgumentException("El hash debe tener 60 caracteres.", nameof(passwordHash));

        Id = Guid.NewGuid();
        Nombre = nombre;
        Correo = correo;
        PasswordHash = passwordHash;
        FechaCreacion = DateTime.UtcNow;
        Tareas = new List<Tarea>();
    }
    public void CambiarNombre(string nuevoNombre)
    {
         if(string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre no puede estar vacío.", nameof(nuevoNombre));
            
        if(nuevoNombre.Length < 3)
            throw new ArgumentException("El nombre debe tener al menos 3 caracteres.", nameof(nuevoNombre));

        if (nuevoNombre.Length > 100)
            throw new ArgumentException("El nombre no puede tener más de 100 caracteres.", nameof(nuevoNombre));
            
         Nombre = nuevoNombre.Trim();    
         FechaActualizacion = DateTime.UtcNow;
    }

    public void CambiarContraseña(string newHashPassword)
    {
        if (string.IsNullOrWhiteSpace(newHashPassword))
            throw new ArgumentException("La contraseña no puede estar vacía.", nameof(newHashPassword));
            PasswordHash = newHashPassword;
            FechaActualizacion = DateTime.UtcNow;
    }
    public void Activar()
    {
        if(Activo)
            throw new InvalidOperationException("El usuario ya está activo.");
        Activo = true;
        FechaActualizacion = DateTime.UtcNow;
    }
    public void Desactivar()
    {
        if(!Activo)
            throw new InvalidOperationException("El usuario ya está desactivado.");
        Activo = false;
        FechaActualizacion = DateTime.UtcNow;
    }
}
