namespace todoList.Application.UseCases;

using todoList.Application.DTO;
using todoList.Domain;

public class ObtenerTareaUseCase
{
 private readonly ITareasRepository _repository;
 public ObtenerTareaUseCase(ITareasRepository repository)
 {
     _repository = repository;  
 }
 public async Task<ResponseDto> ObtenerTareaAsync(Guid id)
 {
     var tarea = await _repository.GetByIdAsync(id);
     
     if(tarea == null) throw new KeyNotFoundException("No se encontró el id.");
    
     return new ResponseDto(tarea.Id, tarea.Nombre, tarea.IsCompleted);
}
}
