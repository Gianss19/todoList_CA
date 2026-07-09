using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoList.Application.DTO.Usuario;
using todoList.Application.UseCases.Usuario;
using todoList.Domain;

namespace todoList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsuariosController : ControllerBase
{
    private readonly ActivarUsuarioUseCase _activarUsuario;
    private readonly BorrarUsuarioUseCase _borrarUsuario;
    private readonly CambiarContraseñaUsuarioUseCase _cambiarContraseña;
    private readonly CambiarNombreUsuarioUseCase _cambiarNombre;
    private readonly CrearUsuarioUseCase _crearUsuario;
    private readonly DesactivarUsuarioUseCase _desactivarUsuario;
    private readonly ObtenerTodosUsuariosUseCase _obtenerTodosUsuarios;
    
    public UsuariosController(ActivarUsuarioUseCase activarUsuario,
                              BorrarUsuarioUseCase borrarUsuario,
                              CambiarContraseñaUsuarioUseCase cambiarContraseña,
                              CambiarNombreUsuarioUseCase cambiarNombre,
                              CrearUsuarioUseCase crearUsuario,
                              DesactivarUsuarioUseCase desactivarUsuario,
                              ObtenerTodosUsuariosUseCase obtenerTodosUsuarios)
    {
        _activarUsuario = activarUsuario;
        _borrarUsuario = borrarUsuario;
        _cambiarContraseña = cambiarContraseña;
        _cambiarNombre = cambiarNombre;
        _crearUsuario = crearUsuario;
        _desactivarUsuario = desactivarUsuario;
        _obtenerTodosUsuarios = obtenerTodosUsuarios;    
    }



[HttpPatch("{id:guid}/activar")]
[Authorize(Roles = nameof(Rol.Administrador))]
public async Task<IActionResult> ActivarUsuario(Guid id)
{
    try
    {
        await _activarUsuario.ActivarUsuario(id);
        return NoContent();
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
    [HttpDelete("{id:guid}/borrar")]
    [Authorize(Roles = nameof(Rol.Administrador))]

    public async Task<IActionResult> Borrar(Guid id)
    {
        try
        {
           await _borrarUsuario.BorrarUsuario(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id:guid}/cambiar-contraseña")]
    [Authorize]
    public async Task<IActionResult> CambiarContraseña( [FromBody][Required] CambiarContraseñaRequestDto cambiar)
    {
        try
        {
          await  _cambiarContraseña.CambiarContraseña(cambiar);
            return Ok();
        }
        catch (Exception ex)
        {
            
            return BadRequest(ex.Message);
            
        }
    }

    [HttpPatch("{id:guid}/cambiar-nombre")]
    [Authorize]
    public async Task<IActionResult> CambiarNombre([FromBody][Required] CambiarNombreUsuarioRequestDto cambiarNombre)
    {
        try
        {
            var usuario = await _cambiarNombre.CambiarNombreUsuario(cambiarNombre);

            return Ok(usuario);
            
        }
        catch (Exception ex)
        {
            
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("crear")]
    public async Task<IActionResult> CrearUsuario([FromBody][Required] CrearUsuarioRequestDto newUser)
    {
        try
        {
           await _crearUsuario.Crear(newUser);
           return Created();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }        
    }

    [HttpPatch("{id:guid}/desactivar")]
    [Authorize(Roles =  nameof(Rol.Administrador))]
    public async Task<IActionResult> Desactivar(Guid id)
    {
        try
        {
            await _desactivarUsuario.DesactivarUsuario(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    

    [HttpGet("all")]
    [Authorize(Roles = nameof(Rol.Administrador))]
    public async Task<IActionResult> Get()
    {
        var usuarios = await _obtenerTodosUsuarios.ObtenerTodosUsuarios();
        return Ok(usuarios);
    }


}
