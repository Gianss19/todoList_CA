using System.Collections;
using todoList.Application.DTO.Usuario;
using todoList.Application.Services;
using todoList.Domain;


namespace todoList.Application.UseCases.Usuario;

public class CrearUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IPasswordHasher _hasher;

    public CrearUsuarioUseCase(IUsuarioRepository repository,
                               IPasswordHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }


    public async Task Crear(CrearUsuarioRequestDto request)

    {
        var existe = await _repository.GetByEmailAsync(request.Correo) != null;

        if(existe) throw new InvalidOperationException("El correo ya está en uso.");

        var passwordHash = _hasher.Hash(request.Password);
       
        var nuevoUsuario = new Domain.Usuario(request.Nombre, request.Correo, passwordHash);

       await _repository.AddAsync(nuevoUsuario);
    }

    
}

