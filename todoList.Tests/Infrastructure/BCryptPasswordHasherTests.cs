using FluentAssertions;
using todoList.Infrastructure.Services;

namespace todoList.Tests.Infrastructure;

public class BCryptPasswordHasherTests
{
    private readonly BCryptPasswordHasher _hasher = new();

    [Fact]
    public void Hash_PasswordValido_RetornaHash()
    {
        var hash = _hasher.Hash("Password123!");

        hash.Should().NotBeNullOrEmpty();
        hash.Should().HaveLength(60);
    }

    [Fact]
    public void Hash_MismoPassword_RetornaHashesDiferentes()
    {
        var hash1 = _hasher.Hash("Password123!");
        var hash2 = _hasher.Hash("Password123!");

        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void Verify_PasswordCorrecto_RetornaTrue()
    {
        var hash = _hasher.Hash("Password123!");

        var result = _hasher.Verify("Password123!", hash);

        result.Should().BeTrue();
    }

    [Fact]
    public void Verify_PasswordIncorrecto_RetornaFalse()
    {
        var hash = _hasher.Hash("Password123!");

        var result = _hasher.Verify("OtraPassword!", hash);

        result.Should().BeFalse();
    }
}
