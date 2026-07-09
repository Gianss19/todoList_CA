using System.Diagnostics;
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
    var sw = Stopwatch.StartNew();
    
    var usuario = await _repository.GetByEmailAsync(request.Email);
    sw.Stop();
    
    Console.WriteLine($"Busqueda de usuario: {sw.ElapsedMilliseconds} ms");
    sw.Restart();


    if(usuario == null)
       throw new KeyNotFoundException($"Credenciales Inválidas.");
    //verificar que el password sea correcto

    var auth = _hasher.Verify(request.Password, usuario.PasswordHash);
    
    sw.Stop();
    Console.WriteLine($"Verificación de password: {sw.ElapsedMilliseconds} ms");
    sw.Restart();
    
    if(!auth)
        throw new UnauthorizedAccessException("Credenciales Inválidas.");
    var token = _token.GenerateToken(usuario.Id, usuario.Nombre, usuario.Correo, usuario.rol.ToString());
    
    sw.Stop();
    
    Console.WriteLine($"Generación de Token: {sw.ElapsedMilliseconds} ms");
    
    sw.Restart();
    
    return token;

}
}