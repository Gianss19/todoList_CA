using Microsoft.EntityFrameworkCore;
using todoList.Domain;
using todoList.Infrastructure.Persistence;

namespace todoList.Infrastructure.Repository;


public class EfUsuarioRepository : IUsuarioRepository
{
    private readonly TodoListDbContext _context;
    public EfUsuarioRepository(TodoListDbContext context)
    {
        _context = context;
    }


    public async Task AddAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        
    }

    public async Task DeleteAsync(Guid id)
    {
    
       var usuario = await _context.Usuarios.FindAsync(id);
       if(usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        if(usuario == null)
            throw new InvalidOperationException("El usuario no existe");

        _context.Entry(usuario).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Usuarios.AnyAsync(t => t.Id == id);  
    }

    public async Task<IReadOnlyList<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
                                    .Where(a => a.Activo)
                                    .ToListAsync();
    }

    public async Task<Usuario?> GetByEmailAsync(string correo)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(c=> c.Correo == correo);

        return (usuario==null) ? null : usuario;
        

    }

    public async Task<Usuario?> GetByIdAsync(Guid id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(i=> i.Id == id);
        return (usuario == null) ? null: usuario;
    }



}
