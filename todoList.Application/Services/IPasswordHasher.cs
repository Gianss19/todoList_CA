namespace todoList.Application.Services;

public interface IPasswordHasher
{
    public string Hash(string password);
    public bool Verify(string hashed, string plainText);
}
