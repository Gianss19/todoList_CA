namespace todoList.Application.DTO;

public record CrearTareaResponseDto(Guid Id, string Nombre, bool IsCompleted, DateTime FechaCreacion);
