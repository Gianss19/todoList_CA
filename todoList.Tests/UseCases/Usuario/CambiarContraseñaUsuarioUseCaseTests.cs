using FluentAssertions;
using Moq;
using todoList.Application.DTO.Usuario;
using todoList.Application.Services;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Usuario;

public class CambiarContraseñaUsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly Mock<IPasswordHasher> _hasher;
    private readonly CambiarContraseñaUsuarioUseCase _useCase;

    public CambiarContraseñaUsuarioUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _hasher = new Mock<IPasswordHasher>();
        _useCase = new CambiarContraseñaUsuarioUseCase(_repo.Object, _hasher.Object);
    }

    [Fact]
    public async Task CambiarContraseña_UsuarioExiste_CambiaHash()
    {
        var id = Guid.NewGuid();
        var usuario = new TodoDomain.Usuario("Juan", "juan@test.com", new string('x', 60));
        _repo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Hash("NuevaPass123!")).Returns(new string('y', 60));

        await _useCase.CambiarContraseña(new CambiarContraseñaRequestDto(id, "NuevaPass123!"));

        usuario.PasswordHash.Should().Be(new string('y', 60));
        _repo.Verify(r => r.UpdateAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task CambiarContraseña_UsuarioNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var act = () => _useCase.CambiarContraseña(new CambiarContraseñaRequestDto(Guid.NewGuid(), "Pass123!"));

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
