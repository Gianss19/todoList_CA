using todoList.Domain;

namespace todoList.Application.UseCases.Usuario;

public class DesactivarUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;

    public DesactivarUsuarioUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task DesactivarUsuario(Guid id)
    {
        if(!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException("El usuario no existe");

        var usuario = await _repository.GetByIdAsync(id);
            
            usuario.Desactivar();

        await _repository.UpdateAsync(usuario);

    }
}
