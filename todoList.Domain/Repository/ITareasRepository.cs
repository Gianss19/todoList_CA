namespace todoList.Domain;

public interface ITareasRepository
{
    Task AddAsync(Tarea tarea); 

    Task UpdateAsync(Tarea tarea);

    Task<Tarea?> GetByIdAsync(Guid id);

    Task<IReadOnlyList<Tarea>> GetAllAsync();

    Task DeleteAsync(Guid id);
}