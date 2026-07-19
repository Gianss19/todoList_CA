using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Tarea;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Tarea;

public class CompletarTareaUseCaseTests
{
    private readonly Mock<ITareasRepository> _repo;
    private readonly CompletarTareaUseCase _useCase;

    public CompletarTareaUseCaseTests()
    {
        _repo = new Mock<ITareasRepository>();
        _useCase = new CompletarTareaUseCase(_repo.Object);
    }

    [Fact]
    public async Task CompletarTareaAsync_TareaExiste_Completa()
    {
        var id = Guid.NewGuid();
        var tarea = new TodoDomain.Tarea("Mi tarea", Guid.NewGuid());
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tarea);

        var result = await _useCase.CompletarTareaAsync(id);

        result.IsCompleted.Should().BeTrue();
        result.FechaActualizacion.Should().NotBeNull();
        _repo.Verify(r => r.UpdateAsync(tarea), Times.Once);
    }

    [Fact]
    public async Task CompletarTareaAsync_TareaNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TodoDomain.Tarea?)null);

        var act = () => _useCase.CompletarTareaAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
