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

    public async Task<ResponseDto> CambiarNombreAsync(Guid id, CambiarNombreRequestDto request)
    {
        var tarea = await _repository.GetByIdAsync(id);
        
        if(tarea == null) throw new KeyNotFoundException("no se econtró el id.");

        tarea.CambiarNombre(request.nuevoNombre);

        await _repository.UpdateAsync(tarea);

        return new ResponseDto(tarea.Id, tarea.Nombre, tarea.IsCompleted);    
    }
}
