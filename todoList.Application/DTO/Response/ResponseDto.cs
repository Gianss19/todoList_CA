namespace todoList.Application.DTO;

public record ResponseDto(Guid Id, string Nombre, bool IsCompleted);
