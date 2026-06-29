namespace todoList.Application.UseCases;
using todoList.Application.DTO;
using todoList.Domain;
public class CompletarTareaUseCase
{
    private readonly ITareasRepository _repository;

public CompletarTareaUseCase(ITareasRepository repository)
{
    _repository = repository;   
}
    public async Task<ResponseDto> CompletarTareaAsync(Guid id)
    {
        var tarea = await _repository.GetByIdAsync(id);
        
        if(tarea == null) throw new KeyNotFoundException("No se encontró el id.");

        tarea.Completar();
        
        await _repository.UpdateAsync(tarea);
        return new ResponseDto(tarea.Id, tarea.Nombre, tarea.IsCompleted);
    }
}
