using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Tarea;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Tarea;

public class BorrarTareasUseCaseTests
{
    private readonly Mock<ITareasRepository> _repo;
    private readonly BorrarTareasUseCase _useCase;

    public BorrarTareasUseCaseTests()
    {
        _repo = new Mock<ITareasRepository>();
        _useCase = new BorrarTareasUseCase(_repo.Object);
    }

    [Fact]
    public async Task DeleteAsync_TareaExiste_Elimina()
    {
        var id = Guid.NewGuid();
        var tarea = new TodoDomain.Tarea("Mi tarea", Guid.NewGuid());
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tarea);

        await _useCase.DeleteAsync(id);

        _repo.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_TareaNoExiste_LanzaKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TodoDomain.Tarea?)null);

        var act = () => _useCase.DeleteAsync(id);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
