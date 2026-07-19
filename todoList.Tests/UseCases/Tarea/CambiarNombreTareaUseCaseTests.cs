using FluentAssertions;
using Moq;
using todoList.Application.DTO.Tarea;
using todoList.Application.UseCases.Tarea;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Tarea;

public class CambiarNombreTareaUseCaseTests
{
    private readonly Mock<ITareasRepository> _repo;
    private readonly CambiarNombreTareaUseCase _useCase;

    public CambiarNombreTareaUseCaseTests()
    {
        _repo = new Mock<ITareasRepository>();
        _useCase = new CambiarNombreTareaUseCase(_repo.Object);
    }

    [Fact]
    public async Task CambiarNombreAsync_TareaExiste_CambiaNombre()
    {
        var id = Guid.NewGuid();
        var tarea = new TodoDomain.Tarea("Nombre viejo", Guid.NewGuid());
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tarea);

        var result = await _useCase.CambiarNombreAsync(id, new CambiarNombreTareaRequestDto(id, "Nombre nuevo"));

        result.nuevoNombre.Should().Be("Nombre nuevo");
        _repo.Verify(r => r.UpdateAsync(tarea), Times.Once);
    }

    [Fact]
    public async Task CambiarNombreAsync_TareaNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TodoDomain.Tarea?)null);

        var act = () => _useCase.CambiarNombreAsync(Guid.NewGuid(), new CambiarNombreTareaRequestDto(Guid.NewGuid(), "Nuevo"));

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
