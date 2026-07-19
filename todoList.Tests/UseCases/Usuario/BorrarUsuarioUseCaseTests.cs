using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Usuario;

public class BorrarUsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly BorrarUsuarioUseCase _useCase;

    public BorrarUsuarioUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _useCase = new BorrarUsuarioUseCase(_repo.Object);
    }

    [Fact]
    public async Task BorrarUsuario_UsuarioExiste_Elimina()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);

        await _useCase.BorrarUsuario(id);

        _repo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task BorrarUsuario_UsuarioNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var act = () => _useCase.BorrarUsuario(Guid.NewGuid());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
