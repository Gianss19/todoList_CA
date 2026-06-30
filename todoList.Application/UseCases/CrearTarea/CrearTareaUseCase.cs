namespace todoList.Application.UseCases;

using todoList.Application.DTO;

using todoList.Domain;
public class CrearTareaUseCase
{
    private readonly ITareasRepository _repository;
    public CrearTareaUseCase(ITareasRepository repository)
    {
        _repository = repository;
    }


    public async Task<ResponseDto> CrearTareaAsync(CrearTareaRequestDto crearTareaRequestDto)
    {
        if (await _repository.ExistsByNameAsync(crearTareaRequestDto.Nombre))
            throw new InvalidOperationException("Ya existe una tarea con ese nombre.");

        
        Tarea tarea = new(crearTareaRequestDto.Nombre);
        await _repository.AddAsync(tarea);
        
        return new ResponseDto(tarea.Id, tarea.Nombre, tarea.IsCompleted);
    }
}