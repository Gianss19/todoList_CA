using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using todoList.Application.DTO;
using todoList.Application.DTO.Tarea;
using todoList.Application.UseCases.Tarea;
using todoList.Application.Services;
using System.ComponentModel.DataAnnotations;

namespace todoList.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TareasController : ControllerBase
{
    private readonly ObtenerTodasTareasUseCase _obtenerTodasUseCase;
    private readonly ObtenerTareaUseCase _obtenerTareaUseCase;
    private readonly CrearTareaUseCase _crearTareaUseCase;
    private readonly CambiarNombreUseCase _cambiarNombreUseCase;
    private readonly BorrarTareasUseCase _borrarTareasUseCase;
    private readonly CompletarTareaUseCase _completarTareaUseCase;
    private readonly IHttpCatService _httpCatService;
    

    public TareasController(
        ObtenerTodasTareasUseCase obtenerTodasUseCase,
        ObtenerTareaUseCase obtenerTareaUseCase,
        CrearTareaUseCase crearTareaUseCase,
        CambiarNombreUseCase cambiarNombreUseCase,
        BorrarTareasUseCase borrarTareasUseCase,
        CompletarTareaUseCase completarTareaUseCase,
        IHttpCatService httpCatService)
    {
        _obtenerTodasUseCase = obtenerTodasUseCase;
        _obtenerTareaUseCase = obtenerTareaUseCase;
        _crearTareaUseCase = crearTareaUseCase;
        _cambiarNombreUseCase = cambiarNombreUseCase;
        _borrarTareasUseCase = borrarTareasUseCase;
        _completarTareaUseCase = completarTareaUseCase;
        _httpCatService = httpCatService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ResponseDto>>> GetAll()
    {
        var tareas = await _obtenerTodasUseCase.ObtenerTodasAsync();
        return Ok(tareas);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ResponseDto>> GetById(Guid id)
    {
        try
        {
            var tarea = await _obtenerTareaUseCase.ObtenerTareaAsync(id);
            return Ok(tarea);
        }
        catch (KeyNotFoundException)
        {
            var catUrl = await _httpCatService.GetImageUrlAsync(400);
            return Redirect(catUrl);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ResponseDto>> Create([FromBody] [Required] CrearTareaRequestDto crearTareaRequestDto)
    {
        try
        {
            var response = await _crearTareaUseCase.CrearTareaAsync(crearTareaRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException)
        {
            var catUrl = await _httpCatService.GetImageUrlAsync(409);
            return Redirect(catUrl);
        }
    }
    [HttpPatch("{id:guid}/nombre")]
    public async Task<ActionResult<ResponseDto>> NameUpdate(Guid id, [FromBody] [Required] CambiarNombreTareaRequestDto cambiarNombreRequestDto)
    {
        try
        {
            var response = await _cambiarNombreUseCase.CambiarNombreAsync(id, cambiarNombreRequestDto);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            
            return NotFound();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            var response = _borrarTareasUseCase.DeleteAsync(id);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            
            return NotFound();
        }
        

    }
    [HttpPost("{id:guid}/completar")]
    public async Task<ActionResult<ResponseDto>>CompleteTask(Guid id)
    {
        try
        {
            var response = await _completarTareaUseCase.CompletarTareaAsync(id);
            return Ok(response);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


}