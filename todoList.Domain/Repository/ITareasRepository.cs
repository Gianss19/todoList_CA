namespace todoList.Domain;

public interface ITareasRepository
{
    Task AddAsync(Tarea tarea); 

    Task UpdateAsync(Tarea tarea);

    Task<Tarea?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Tarea>> GetTaskByUserAsync(Guid userId); // FK para el usuario que la creo

    Task<IReadOnlyList<Tarea>> GetAllAsync();
    Task<bool> ExistsAsync(Guid id);

    Task DeleteAsync(Guid id);
}