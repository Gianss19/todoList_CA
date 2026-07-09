using Microsoft.EntityFrameworkCore;
using todoList.Domain;
using todoList.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace todoList.Infrastructure.Repository;

public class EfTareaRepository : ITareasRepository
{
    
    private readonly TodoListDbContext _context;

    public EfTareaRepository(TodoListDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Tarea tarea)
    {
       await _context.Tareas.AddAsync(tarea);
       await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var usuario = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == id);
        
        if(usuario == null)
            throw new InvalidOperationException("El usuario no existe");
            
             _context.Tareas.Remove(usuario);
       await _context.SaveChangesAsync();        
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Tareas.AnyAsync(t => t.Id == id);        
    }

    public async Task<bool> ExistsByNameAsync(string nombre)
    {
        return await _context.Tareas.AnyAsync(t => t.Nombre == nombre);
    }

    public async Task<IReadOnlyList<Tarea>> GetAllAsync()
    {
        var tareas = from lista
                     in _context.Tareas
                     select lista;

        return await tareas.ToListAsync();        
    }
 

    public async Task<IReadOnlyList<Tarea>> GetAllTasksByUserAsync(Guid userId)
    {
        var tareas = from lista in _context.Tareas
                     where lista.UsuarioId == userId
                     select lista;

        return await tareas.ToListAsync();
    }

    public async Task<Tarea?> GetByIdAsync(Guid id)
    {
       var tarea = await _context.Tareas.FindAsync(id);
       return (tarea == null)? null : tarea; 
    }

    public async Task UpdateAsync(Tarea tarea)
    {
        _context.Entry(tarea).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

}
