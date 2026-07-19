using todoList.Domain;

namespace todoList.Application.UseCases.Usuario;

public class BorrarUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    public BorrarUsuarioUseCase(IUsuarioRepository repository)
    {
        _repository = repository;   
    }

    public async Task BorrarUsuario(Guid id)
    {
        if(!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException("El usuario no existe.");

        await _repository.DeleteAsync(id);
    }
}
