namespace todoList.Application.DTO;

public record CambiarNombreTareaResponseDto(Guid id, string nuevoNombre, DateTime? FechaActualizacion);
