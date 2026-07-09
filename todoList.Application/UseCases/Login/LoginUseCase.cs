using todoList.Application.DTO.Login;
using todoList.Application.Services;
using todoList.Domain;

namespace todoList.Application.UseCases.Login;

public class LoginUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _token;
    public LoginUseCase(IUsuarioRepository repository,
                        IPasswordHasher hasher,
                        ITokenService token)
    {
        _repository = repository;
        _hasher = hasher;
        _token = token;
    }

public async Task<string> Login(LoginRequestDto request)
{
    var usuario = await _repository.GetByEmailAsync(request.Email);
    if(usuario == null)
       throw new KeyNotFoundException($"Credenciales Inválidas.");
    //verificar que el password sea correcto

    var auth = _hasher.Verify(usuario.PasswordHash, request.Password);
    if(!auth)
        throw new UnauthorizedAccessException("Credenciales Inválidas.");
    var token = _token.GenerateToken(usuario.Id, usuario.Nombre, usuario.Correo, usuario.rol.ToString());

    return token;

}
}