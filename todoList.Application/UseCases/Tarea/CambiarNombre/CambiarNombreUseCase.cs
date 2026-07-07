
namespace todoList.Application.UseCases.Tarea;
using todoList.Application.DTO;
using todoList.Application.DTO.Tarea;
using todoList.Domain;


public class CambiarNombreUseCase
{
    private readonly ITareasRepository _repository;

    public CambiarNombreUseCase(ITareasRepository repository)
    {
        _repository = repository;
    }    

    public async Task<CambiarNombreTareaResponseDto> CambiarNombreAsync(Guid id, CambiarNombreTareaRequestDto request)
    {
        var tarea = await _repository.GetByIdAsync(id);
        
        if(tarea == null) throw new KeyNotFoundException("no se econtró el id.");

        tarea.CambiarNombre(request.nuevoNombre);

        await _repository.UpdateAsync(tarea);

        return new CambiarNombreTareaResponseDto(tarea.Id, tarea.Nombre, DateTime.Now);    
    }
}
