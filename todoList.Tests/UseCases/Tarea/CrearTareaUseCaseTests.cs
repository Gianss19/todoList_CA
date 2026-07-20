using FluentAssertions;
using Moq;
using todoList.Application.DTO.Tarea;
using todoList.Application.UseCases.Tarea;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Tarea;

public class CrearTareaUseCaseTests
{
    private readonly Mock<ITareasRepository> _tareasRepo;
    private readonly Mock<IUsuarioRepository> _usuarioRepo;
    private readonly CrearTareaUseCase _useCase;

    public CrearTareaUseCaseTests()
    {
        _tareasRepo = new Mock<ITareasRepository>();
        _usuarioRepo = new Mock<IUsuarioRepository>();
        _useCase = new CrearTareaUseCase(_tareasRepo.Object, _usuarioRepo.Object);
    }

    [Fact]
    public async Task CrearTareaAsync_DatosValidos_CreaYRetornaDto()
    {
        var userId = Guid.NewGuid();
        _tareasRepo.Setup(r => r.ExistsByNameAsync("Mi tarea")).ReturnsAsync(false);
        _usuarioRepo.Setup(r => r.ExistsAsync(userId)).ReturnsAsync(true);

        var result = await _useCase.CrearTareaAsync(new CrearTareaRequestDto("Mi tarea", userId));

        result.Nombre.Should().Be("Mi tarea");
        result.IsCompleted.Should().BeFalse();
        result.Id.Should().NotBe(Guid.Empty);
        _tareasRepo.Verify(r => r.AddAsync(It.IsAny<TodoDomain.Tarea>()), Times.Once);
    }

    [Fact]
    public async Task CrearTareaAsync_NombreDuplicado_LanzaInvalidOperationException()
    {
        _tareasRepo.Setup(r => r.ExistsByNameAsync("Duplicada")).ReturnsAsync(true);

        var act = () => _useCase.CrearTareaAsync(new CrearTareaRequestDto("Duplicada", Guid.NewGuid()));

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*ya existe*");
    }

    [Fact]
    public async Task CrearTareaAsync_UsuarioNoExiste_LanzaInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        _tareasRepo.Setup(r => r.ExistsByNameAsync("Mi tarea")).ReturnsAsync(false);
        _usuarioRepo.Setup(r => r.ExistsAsync(userId)).ReturnsAsync(false);

        var act = () => _useCase.CrearTareaAsync(new CrearTareaRequestDto("Mi tarea", userId));

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*usuario no existe*");
    }
}
