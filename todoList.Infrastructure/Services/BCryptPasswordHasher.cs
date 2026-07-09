using todoList.Application.Services;
using BCrypt.Net;
namespace todoList.Infrastructure.Services;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        var HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        return HashedPassword;
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
        
    }

}
