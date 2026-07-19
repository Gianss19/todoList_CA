using FluentAssertions;
using Moq;
using todoList.Application.DTO.Usuario;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Usuario;

public class CambiarNombreUsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly CambiarNombreUsuarioUseCase _useCase;

    public CambiarNombreUsuarioUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _useCase = new CambiarNombreUsuarioUseCase(_repo.Object);
    }

    [Fact]
    public async Task CambiarNombreUsuario_UsuarioExiste_CambiaNombre()
    {
        var id = Guid.NewGuid();
        var usuario = new TodoDomain.Usuario("Juan", "juan@test.com", new string('x', 60));
        _repo.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(usuario);

        var result = await _useCase.CambiarNombreUsuario(new CambiarNombreUsuarioRequestDto(id, "Pedro"));

        result.Nombre.Should().Be("Pedro");
        _repo.Verify(r => r.UpdateAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task CambiarNombreUsuario_UsuarioNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var act = () => _useCase.CambiarNombreUsuario(new CambiarNombreUsuarioRequestDto(Guid.NewGuid(), "Pedro"));

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
