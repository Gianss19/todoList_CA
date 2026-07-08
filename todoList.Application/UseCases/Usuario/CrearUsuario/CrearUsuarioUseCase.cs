using todoList.Domain;

namespace todoList.Application.UseCases.Usuario;

public class CrearUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;

    public CrearUsuarioUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    
}

