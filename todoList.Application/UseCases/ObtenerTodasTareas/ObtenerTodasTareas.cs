namespace todoList.Application.UseCases;

using todoList.Application.DTO;

using todoList.Domain;

public class ObtenerTodasTareasUseCase
{
    private readonly ITareasRepository _repository;
    public ObtenerTodasTareasUseCase(ITareasRepository repository)
    {
        _repository = repository;   
    }

    public async Task<IReadOnlyList<ResponseDto>> ObtenerTodasAsync()
    {
        var tareas = await _repository.GetAllAsync();
        
        return tareas.Select(t => new ResponseDto(t.Id, t.Nombre, t.IsCompleted)).ToList();
    }
}
