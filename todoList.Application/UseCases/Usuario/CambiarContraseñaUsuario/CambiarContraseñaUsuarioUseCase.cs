using System.Security;
using todoList.Application.DTO.Usuario;
using todoList.Domain;

namespace todoList.Application.UseCases.Usuario;

public class CambiarContraseñaUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    public CambiarContraseñaUsuarioUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }


    public async Task CambiarContraseña(CambiarContraseñaRequestDto request)
    {
        if(!await _repository.ExistsAsync(request.id))
            throw new KeyNotFoundException("El usuario no existe.");

        var usuario = await _repository.GetByIdAsync(request.id);
        
        usuario.CambiarContraseña(request.nuevoHash);
        
        await _repository.UpdateAsync(usuario);
    }
}
