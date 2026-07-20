using FluentAssertions;
using Moq;
using todoList.Application.UseCases.Tarea;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Tarea;

public class ObtenerTodasTareasPorUsuarioUseCaseTests
{
    private readonly Mock<ITareasRepository> _tareasRepo;
    private readonly Mock<IUsuarioRepository> _usuarioRepo;
    private readonly ObtenerTodasTareasPorUsuarioUseCase _useCase;

    public ObtenerTodasTareasPorUsuarioUseCaseTests()
    {
        _tareasRepo = new Mock<ITareasRepository>();
        _usuarioRepo = new Mock<IUsuarioRepository>();
        _useCase = new ObtenerTodasTareasPorUsuarioUseCase(_tareasRepo.Object, _usuarioRepo.Object);
    }

    [Fact]
    public async Task ObtenerTodasPorUsuarioAsync_UsuarioExiste_RetornaTareas()
    {
        var userId = Guid.NewGuid();
        _usuarioRepo.Setup(r => r.ExistsAsync(userId)).ReturnsAsync(true);
        var tareas = new List<TodoDomain.Tarea> { new("Tarea 1", userId) };
        _tareasRepo.Setup(r => r.GetAllTasksByUserAsync(userId)).ReturnsAsync(tareas);

        var result = await _useCase.ObtenerTodasPorUsuarioAsync(userId);

        result.Should().HaveCount(1);
        result[0].Nombre.Should().Be("Tarea 1");
    }

    [Fact]
    public async Task ObtenerTodasPorUsuarioAsync_UsuarioNoExiste_LanzaDataException()
    {
        _usuarioRepo.Setup(r => r.ExistsAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var act = () => _useCase.ObtenerTodasPorUsuarioAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<System.Data.DataException>();
    }
}
