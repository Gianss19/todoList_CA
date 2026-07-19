using FluentAssertions;
using Moq;
using todoList.Application.DTO.Usuario;
using todoList.Application.Services;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Usuario;

public class CrearUsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly Mock<IPasswordHasher> _hasher;
    private readonly CrearUsuarioUseCase _useCase;

    public CrearUsuarioUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _hasher = new Mock<IPasswordHasher>();
        _useCase = new CrearUsuarioUseCase(_repo.Object, _hasher.Object);
    }

    [Fact]
    public async Task Crear_DatosValidos_CreaUsuario()
    {
        _repo.Setup(r => r.GetByEmailAsync("nuevo@test.com")).ReturnsAsync((TodoDomain.Usuario?)null);
        _hasher.Setup(h => h.Hash("Password123!")).Returns(new string('x', 60));

        await _useCase.Crear(new CrearUsuarioRequestDto("Nuevo", "nuevo@test.com", "Password123!"));

        _repo.Verify(r => r.AddAsync(It.IsAny<TodoDomain.Usuario>()), Times.Once);
    }

    [Fact]
    public async Task Crear_CorreoDuplicado_LanzaInvalidOperationException()
    {
        var existente = new TodoDomain.Usuario("Existente", "test@test.com", new string('x', 60));
        _repo.Setup(r => r.GetByEmailAsync("test@test.com")).ReturnsAsync(existente);

        var act = () => _useCase.Crear(new CrearUsuarioRequestDto("Nuevo", "test@test.com", "Password123!"));

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*correo ya está en uso*");
    }
}
