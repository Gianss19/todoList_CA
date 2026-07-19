using FluentAssertions;
using Moq;
using todoList.Application.DTO.Login;
using todoList.Application.Services;
using todoList.Application.UseCases.Login;
using todoList.Domain;
using TodoDomain = todoList.Domain;

namespace todoList.Tests.UseCases.Login;

public class LoginUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repo;
    private readonly Mock<IPasswordHasher> _hasher;
    private readonly Mock<ITokenService> _token;
    private readonly LoginUseCase _useCase;

    public LoginUseCaseTests()
    {
        _repo = new Mock<IUsuarioRepository>();
        _hasher = new Mock<IPasswordHasher>();
        _token = new Mock<ITokenService>();
        _useCase = new LoginUseCase(_repo.Object, _hasher.Object, _token.Object);
    }

    [Fact]
    public async Task Login_CredencialesValidas_RetornaToken()
    {
        var usuario = new TodoDomain.Usuario("Juan", "juan@test.com", new string('x', 60));
        _repo.Setup(r => r.GetByEmailAsync("juan@test.com")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("password123", usuario.PasswordHash)).Returns(true);
        _token.Setup(t => t.GenerateToken(usuario.Id, usuario.Nombre, usuario.Correo, usuario.rol.ToString()))
              .Returns("jwt-token");

        var result = await _useCase.Login(new LoginRequestDto("juan@test.com", "password123"));

        result.Should().Be("jwt-token");
    }

    [Fact]
    public async Task Login_EmailNoExiste_LanzaKeyNotFoundException()
    {
        _repo.Setup(r => r.GetByEmailAsync("noexiste@test.com")).ReturnsAsync((TodoDomain.Usuario?)null);

        var act = () => _useCase.Login(new LoginRequestDto("noexiste@test.com", "password"));

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Login_PasswordIncorrecto_LanzaUnauthorizedAccessException()
    {
        var usuario = new TodoDomain.Usuario("Juan", "juan@test.com", new string('x', 60));
        _repo.Setup(r => r.GetByEmailAsync("juan@test.com")).ReturnsAsync(usuario);
        _hasher.Setup(h => h.Verify("wrongpassword", usuario.PasswordHash)).Returns(false);

        var act = () => _useCase.Login(new LoginRequestDto("juan@test.com", "wrongpassword"));

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
