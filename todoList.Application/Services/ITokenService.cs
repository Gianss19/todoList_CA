namespace todoList.Application.Services;

public interface ITokenService
{
    public string GenerateToken(Guid id, string nombre, string correo, string rol);
}
