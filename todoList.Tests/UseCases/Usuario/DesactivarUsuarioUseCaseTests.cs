using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Usuario;

public class DesactivarUsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly DesactivarUsuarioUseCase _useCase;

    public DesactivarUsuarioUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _useCase = new DesactivarUsuarioUseCase(_repo.Object);
    }

    [Fact]
    public async Task DesactivarUsuario_UsuarioActivo_Desactiva()
    {
        var id = Guid.NewGuid();
        var usuario = new TodoDomain.Usuario("Juan", "juan@test.com", new string('x', 60));

        _repo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(usuario);

        await _useCase.DesactivarUsuario(id);

        usuario.Activo.Should().BeFalse();
        _repo.Verify(r => r.UpdateAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task DesactivarUsuario_UsuarioNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var act = () => _useCase.DesactivarUsuario(Guid.NewGuid());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
