using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Usuario;

public class ActivarUsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly ActivarUsuarioUseCase _useCase;

    public ActivarUsuarioUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _useCase = new ActivarUsuarioUseCase(_repo.Object);
    }

    [Fact]
    public async Task ActivarUsuario_UsuarioInactivo_Activa()
    {
        var id = Guid.NewGuid();
        var usuario = new TodoDomain.Usuario("Juan", "juan@test.com", new string('x', 60));
        usuario.Desactivar();

        _repo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(usuario);

        await _useCase.ActivarUsuario(id);

        usuario.Activo.Should().BeTrue();
        _repo.Verify(r => r.UpdateAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task ActivarUsuario_UsuarioNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var act = () => _useCase.ActivarUsuario(Guid.NewGuid());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
