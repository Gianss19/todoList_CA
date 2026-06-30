namespace todoList.Application.UseCases.Usuario;
using todoList.Domain;
public class ObtenerTodosUsuariosUseCase
{
    private readonly IUsuarioRepository _repository;

    public ObtenerTodosUsuariosUseCase(IUsuarioRepository repository)
    {
        _repository = repository;
    }
}
