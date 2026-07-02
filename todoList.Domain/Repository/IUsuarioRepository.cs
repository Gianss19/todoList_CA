namespace todoList.Domain;

public interface IUsuarioRepository
{
    Task AddAsync(Usuario usuario); 

    Task UpdateAsync(Usuario usuario);

    Task<Usuario?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<Usuario?> GetByEmailAsync(string correo);
    Task<IReadOnlyList<Usuario>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
