using System.Security;
using todoList.Application.DTO.Usuario;
using todoList.Application.Services;
using todoList.Domain;

namespace todoList.Application.UseCases.Usuario;

public class CambiarContraseñaUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    public CambiarContraseñaUsuarioUseCase(IUsuarioRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }


    public async Task CambiarContraseña(CambiarContraseñaRequestDto request)
    {
        if(!await _repository.ExistsAsync(request.id))
            throw new KeyNotFoundException("El usuario no existe.");
        var usuario = await _repository.GetByIdAsync(request.id);  //mejorar: implementar verificacion de email antes de cambiar password.

        var nuevoHash = _passwordHasher.Hash(request.NuevaPassword);
        
        usuario.CambiarContraseña(nuevoHash);
        
        await _repository.UpdateAsync(usuario);
    }
}
