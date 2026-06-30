using todoList.Domain;

namespace todoList.Application.UseCases.Usuario.CrearUsuario;

public class CrearUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;

    public CrearUsuarioUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    
}

