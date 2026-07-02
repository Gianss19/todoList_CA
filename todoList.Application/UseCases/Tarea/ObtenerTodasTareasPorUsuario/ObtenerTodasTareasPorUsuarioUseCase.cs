namespace todoList.Application.UseCases.Tarea;

using System.Data;
using todoList.Application.DTO;
using todoList.Domain;


public class ObtenerTodasTareasPorUsuarioUseCase
{
    private readonly ITareasRepository _tareasRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    public ObtenerTodasTareasPorUsuarioUseCase(ITareasRepository tareasRepository,
                                               IUsuarioRepository usuarioRepository )
    {
        _tareasRepository = tareasRepository;   
        _usuarioRepository = usuarioRepository;
    }
    public async Task<IReadOnlyList<GeneralTareaResponseDto>> ObtenerTodasPorUsuarioAsync(Guid userId)
    {  
        if(!await _usuarioRepository.ExistsAsync(userId))
           throw new DataException("No se encontró el id del usuario.");

        var tareas = await _tareasRepository.GetAllTasksByUserAsync(userId);

        return tareas.Select(t=> new GeneralTareaResponseDto(t.Id, t.Nombre, t.IsCompleted, t.FechaCreacion)).ToList();
    }
}
