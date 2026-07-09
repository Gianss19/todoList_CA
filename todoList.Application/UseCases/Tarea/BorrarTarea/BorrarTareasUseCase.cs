using Microsoft.VisualBasic;
using todoList.Domain;

namespace todoList.Application.UseCases.Tarea;

public class BorrarTareasUseCase
{
    private readonly ITareasRepository _repository;

    public BorrarTareasUseCase(ITareasRepository repository)
    {
        _repository = repository;
    }
    public async Task DeleteAsync(Guid id)
    {
        var tarea = await _repository.GetByIdAsync(id);
        
        if(tarea == null) throw new KeyNotFoundException("no se econtró el id.");
        
        await _repository.DeleteAsync(id);

    }
}



