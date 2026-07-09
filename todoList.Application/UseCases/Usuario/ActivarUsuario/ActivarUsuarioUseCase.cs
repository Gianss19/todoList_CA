using System.Diagnostics.CodeAnalysis;
using todoList.Domain;

namespace todoList.Application.UseCases.Usuario;

public class ActivarUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    public ActivarUsuarioUseCase(IUsuarioRepository repository)
    {
        _repository = repository;   
    }

    public async Task ActivarUsuario(Guid id)
    {
        if(!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException("No existe el usuario.");
        
        var usuario = await _repository.GetByIdAsync(id);
        usuario.Activar();

        await _repository.UpdateAsync(usuario);
        
        
    }
}
