namespace todoList.Application.UseCases.Usuario;

using todoList.Application.DTO.Usuario;

using todoList.Domain;
public class ObtenerTodosUsuariosUseCase
{
    private readonly IUsuarioRepository _repository;

    public ObtenerTodosUsuariosUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<GeneralUsuarioResponseDto>> ObtenerTodosUsuarios()
    {
        var usuarios = await _repository.GetAllAsync();
        
        if(!usuarios.Any())
            return new List<GeneralUsuarioResponseDto>();
        
        return usuarios.Select(t=> new GeneralUsuarioResponseDto(t.Id, t.Nombre, t.Correo, t.Activo, t.FechaCreacion)).ToList();

    }
}
