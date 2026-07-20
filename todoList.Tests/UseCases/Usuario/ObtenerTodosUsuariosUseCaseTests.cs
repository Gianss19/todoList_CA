using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Usuario;

public class ObtenerTodosUsuariosUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly ObtenerTodosUsuariosUseCase _useCase;

    public ObtenerTodosUsuariosUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _useCase = new ObtenerTodosUsuariosUseCase(_repo.Object);
    }

    [Fact]
    public async Task ObtenerTodosUsuarios_HayUsuarios_RetornaLista()
    {
        var usuarios = new List<TodoDomain.Usuario>
        {
            new("Juan", "juan@test.com", new string('x', 60)),
            new("Pedro", "pedro@test.com", new string('y', 60))
        };
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(usuarios);

        var result = await _useCase.ObtenerTodosUsuarios();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ObtenerTodosUsuarios_NoHayUsuarios_RetornaListaVacia()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TodoDomain.Usuario>());

        var result = await _useCase.ObtenerTodosUsuarios();

        result.Should().BeEmpty();
    }
}
