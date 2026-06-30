namespace todoList.Application.UseCases.Usuario;
using todoList.Domain;
using todoList.Application.DTO.Usuario;
public class CambiarNombreUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    public CambiarNombreUsuarioUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    
 public async Task<GeneralUsuarioResponseDto> CambiarNombreUsuario(CambiarNombreUsuarioRequestDto request)
    {
        if(!await _repository.ExistsAsync(request.id))
            throw new KeyNotFoundException("El usuario no existe.");
        
        var usuario = await _repository.GetByIdAsync(request.id);
        
        usuario.CambiarNombre(request.nuevoNombre);
        await _repository.UpdateAsync(usuario);

        return new GeneralUsuarioResponseDto(usuario.Id, usuario.Nombre, usuario.Correo, usuario.Activo, usuario.FechaCreacion);

    }
}