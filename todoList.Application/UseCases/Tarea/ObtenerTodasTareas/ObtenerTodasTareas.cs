
namespace todoList.Application.UseCases.Tarea;

using todoList.Application.DTO;

using todoList.Domain;

public class ObtenerTodasTareasUseCase
{
    private readonly ITareasRepository _repository;
    public ObtenerTodasTareasUseCase(ITareasRepository repository)
    {
        _repository = repository;   
    }

    public async Task<IReadOnlyList<GeneralTareaResponseDto>> ObtenerTodasAsync()
    {
        var tareas = await _repository.GetAllAsync();
        
        return tareas.Select(t => new GeneralTareaResponseDto(t.Id, t.Nombre, t.IsCompleted, t.FechaCreacion)).ToList();
    }
}
