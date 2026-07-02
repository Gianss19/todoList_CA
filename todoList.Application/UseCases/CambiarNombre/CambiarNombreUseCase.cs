namespace todoList.Application.UseCases;
using todoList.Application.DTO;
using todoList.Domain;


public class CambiarNombreUseCase
{
    private readonly ITareasRepository _repository;

    public CambiarNombreUseCase(ITareasRepository repository)
    {
        _repository = repository;
    }    

    public async Task<ResponseDto> CambiarNombrAsync(Guid id, string nuevoNombre)
    {
        var tarea = await _repository.GetByIdAsync(id);
        
        if(tarea == null) throw new KeyNotFoundException("no se econtró el id.");

        tarea.CambiarNombre(nuevoNombre);

        await _repository.UpdateAsync(tarea);

        return new ResponseDto(tarea.Id, tarea.Nombre, tarea.IsCompleted);    
    }
}
