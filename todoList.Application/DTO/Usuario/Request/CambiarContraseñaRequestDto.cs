namespace todoList.Application.DTO.Usuario;

public record CambiarContraseñaRequestDto(Guid id,  string Hash, string NuevaPassword);
