using System.Runtime.CompilerServices;

namespace todoList.Application.DTO.Usuario;

public record GeneralUsuarioResponseDto(Guid Id, string Nombre, string Correo, bool Activo, DateTime FechaCreacion);
