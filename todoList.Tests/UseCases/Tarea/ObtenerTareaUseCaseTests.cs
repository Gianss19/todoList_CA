using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Tarea;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Tarea;

public class ObtenerTareaUseCaseTests
{
    private readonly Mock<ITareasRepository> _repo;
    private readonly ObtenerTareaUseCase _useCase;

    public ObtenerTareaUseCaseTests()
    {
        _repo = new Mock<ITareasRepository>();
        _useCase = new ObtenerTareaUseCase(_repo.Object);
    }

    [Fact]
    public async Task ObtenerTareaAsync_Existe_RetornaDto()
    {
        var id = Guid.NewGuid();
        var tarea = new TodoDomain.Tarea("Mi tarea", Guid.NewGuid());
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tarea);

        var result = await _useCase.ObtenerTareaAsync(id);

        result.Id.Should().Be(tarea.Id);
        result.Nombre.Should().Be("Mi tarea");
        result.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task ObtenerTareaAsync_NoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TodoDomain.Tarea?)null);

        var act = () => _useCase.ObtenerTareaAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
