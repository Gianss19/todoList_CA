using FluentAssertions;
using todoList.Domain;

namespace todoList.Tests.Domain;

public class TareaTests
{
    [Fact]
    public void Constructor_ConDatosValidos_CreaTarea()
    {
        var userId = Guid.NewGuid();
        var tarea = new Tarea("Mi tarea", userId);

        tarea.Nombre.Should().Be("Mi tarea");
        tarea.UsuarioId.Should().Be(userId);
        tarea.Id.Should().NotBe(Guid.Empty);
        tarea.IsCompleted.Should().BeFalse();
        tarea.FechaCreacion.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Constructor_NombreVacio_LanzaArgumentException(string? nombre)
    {
        var userId = Guid.NewGuid();
        Action act = () => new Tarea(nombre!, userId);
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(nombre));
    }

    [Fact]
    public void Constructor_NombreMenosDe3Caracteres_LanzaArgumentException()
    {
        var userId = Guid.NewGuid();
        Action act = () => new Tarea("ab", userId);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NombreMasDe100Caracteres_LanzaArgumentException()
    {
        var userId = Guid.NewGuid();
        var nombre = new string('a', 101);
        Action act = () => new Tarea(nombre, userId);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NombreConEspacios_HaceTrim()
    {
        var userId = Guid.NewGuid();
        var tarea = new Tarea("  Mi tarea  ", userId);

        tarea.Nombre.Should().Be("Mi tarea");
    }

    [Fact]
    public void Completar_TareaPendiente_Completa()
    {
        var tarea = new Tarea("Mi tarea", Guid.NewGuid());
        tarea.Completar();

        tarea.IsCompleted.Should().BeTrue();
        tarea.FechaActualizacion.Should().NotBeNull();
    }

    [Fact]
    public void Completar_TareaYaCompletada_NoCambia()
    {
        var tarea = new Tarea("Mi tarea", Guid.NewGuid());
        tarea.Completar();
        var fechaActualizacion = tarea.FechaActualizacion;

        tarea.Completar();

        tarea.IsCompleted.Should().BeTrue();
        tarea.FechaActualizacion.Should().Be(fechaActualizacion);
    }

    [Fact]
    public void CambiarNombre_NombreValido_Actualiza()
    {
        var tarea = new Tarea("Mi tarea", Guid.NewGuid());
        tarea.CambiarNombre("Nuevo nombre");

        tarea.Nombre.Should().Be("Nuevo nombre");
        tarea.FechaActualizacion.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("ab")]
    public void CambiarNombre_NombreInvalido_LanzaArgumentException(string? nombre)
    {
        var tarea = new Tarea("Mi tarea", Guid.NewGuid());
        Action act = () => tarea.CambiarNombre(nombre!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_FechaCreacion_EsUtc()
    {
        var tarea = new Tarea("Mi tarea", Guid.NewGuid());
        tarea.FechaCreacion.Kind.Should().Be(DateTimeKind.Utc);
    }
}
