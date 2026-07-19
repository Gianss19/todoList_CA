using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Tarea;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Tarea;

public class ObtenerTodasTareasUseCaseTests
{
    private readonly Mock<ITareasRepository> _repo;
    private readonly ObtenerTodasTareasUseCase _useCase;

    public ObtenerTodasTareasUseCaseTests()
    {
        _repo = new Mock<ITareasRepository>();
        _useCase = new ObtenerTodasTareasUseCase(_repo.Object);
    }

    [Fact]
    public async Task ObtenerTodasAsync_HayTareas_RetornaLista()
    {
        var userId = Guid.NewGuid();
        var tareas = new List<TodoDomain.Tarea>
        {
            new("Tarea 1", userId),
            new("Tarea 2", userId)
        };
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(tareas);

        var result = await _useCase.ObtenerTodasAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ObtenerTodasAsync_NoHayTareas_RetornaListaVacia()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TodoDomain.Tarea>());

        var result = await _useCase.ObtenerTodasAsync();

        result.Should().BeEmpty();
    }
}
