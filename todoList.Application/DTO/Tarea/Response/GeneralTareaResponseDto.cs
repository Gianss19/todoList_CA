namespace todoList.Application.DTO;

public record GeneralTareaResponseDto(Guid Id, string Nombre, bool IsCompleted, DateTime? FechaActualizacion);