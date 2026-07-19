
namespace todoList.Application.UseCases.Tarea;
using todoList.Application.DTO;
using todoList.Application.DTO.Tarea;
using todoList.Domain;
public class CrearTareaUseCase
{
    private readonly ITareasRepository _tareasRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    public CrearTareaUseCase(ITareasRepository tareasRepository, IUsuarioRepository usuarioRepository)
    {
        _tareasRepository = tareasRepository;
        _usuarioRepository = usuarioRepository;
    }


    public async Task<CrearTareaResponseDto> CrearTareaAsync(CrearTareaRequestDto crearTareaRequestDto)
    {
        if (await _tareasRepository.ExistsByNameAsync(crearTareaRequestDto.Nombre))
            throw new InvalidOperationException("Ya existe una tarea con ese nombre.");

            
        if(!await _usuarioRepository.ExistsAsync(crearTareaRequestDto.Usuario_id))
           throw new InvalidOperationException("El usuario no existe.");         
        
        Tarea tarea = new(crearTareaRequestDto.Nombre, crearTareaRequestDto.Usuario_id);
        await _tareasRepository.AddAsync(tarea);
        
        return new CrearTareaResponseDto(tarea.Id, tarea.Nombre, tarea.IsCompleted, tarea.FechaCreacion);
    }
}