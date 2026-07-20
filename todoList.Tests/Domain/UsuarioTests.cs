using FluentAssertions;
using todoList.Domain;

namespace todoList.Tests.Domain;

public class UsuarioTests
{
    private const string ValidHash = "123456789012345678901234567890123456789012345678901234567890";

    [Fact]
    public void Constructor_ConDatosValidos_CreaUsuario()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);

        usuario.Nombre.Should().Be("Juan");
        usuario.Correo.Should().Be("juan@test.com");
        usuario.PasswordHash.Should().Be(ValidHash);
        usuario.Id.Should().NotBe(Guid.Empty);
        usuario.Activo.Should().BeTrue();
        usuario.rol.Should().Be(Rol.Usuario);
        usuario.FechaCreacion.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Constructor_NombreVacio_LanzaArgumentException(string? nombre)
    {
        Action act = () => new Usuario(nombre!, "test@test.com", ValidHash);
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(nombre));
    }

    [Fact]
    public void Constructor_NombreMenosDe3Caracteres_LanzaArgumentException()
    {
        Action act = () => new Usuario("ab", "test@test.com", ValidHash);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_NombreMasDe100Caracteres_LanzaArgumentException()
    {
        var nombre = new string('a', 101);
        Action act = () => new Usuario(nombre, "test@test.com", ValidHash);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("noemail")]
    [InlineData("test@")]
    public void Constructor_CorreoInvalido_LanzaArgumentException(string? correo)
    {
        Action act = () => new Usuario("Juan", correo!, ValidHash);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Constructor_PasswordVacio_LanzaArgumentException(string? password)
    {
        Action act = () => new Usuario("Juan", "test@test.com", password!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_PasswordLongitudIncorrecta_LanzaArgumentException()
    {
        var hashCorto = new string('x', 30);
        Action act = () => new Usuario("Juan", "test@test.com", hashCorto);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CambiarNombre_NombreValido_Actualiza()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        usuario.CambiarNombre("Pedro");

        usuario.Nombre.Should().Be("Pedro");
        usuario.FechaActualizacion.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("ab")]
    public void CambiarNombre_NombreInvalido_LanzaArgumentException(string? nombre)
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        Action act = () => usuario.CambiarNombre(nombre!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CambiarContraseña_NuevoHash_Actualiza()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        var nuevoHash = new string('y', 60);
        usuario.CambiarContraseña(nuevoHash);

        usuario.PasswordHash.Should().Be(nuevoHash);
        usuario.FechaActualizacion.Should().NotBeNull();
    }

    [Fact]
    public void CambiarContraseña_HashVacio_LanzaArgumentException()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        Action act = () => usuario.CambiarContraseña("");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Activar_UsuarioInactivo_Activa()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        usuario.Desactivar();
        usuario.Activar();

        usuario.Activo.Should().BeTrue();
        usuario.FechaActualizacion.Should().NotBeNull();
    }

    [Fact]
    public void Activar_UsuarioYaActivo_LanzaInvalidOperationException()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        Action act = () => usuario.Activar();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Desactivar_UsuarioActivo_Desactiva()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        usuario.Desactivar();

        usuario.Activo.Should().BeFalse();
        usuario.FechaActualizacion.Should().NotBeNull();
    }

    [Fact]
    public void Desactivar_UsuarioYaInactivo_LanzaInvalidOperationException()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        usuario.Desactivar();
        Action act = () => usuario.Desactivar();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Constructor_FechaCreacion_EsUtc()
    {
        var usuario = new Usuario("Juan", "juan@test.com", ValidHash);
        usuario.FechaCreacion.Kind.Should().Be(DateTimeKind.Utc);
    }
}
