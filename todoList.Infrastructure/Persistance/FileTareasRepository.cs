namespace todoList.Infrastructure;
using todoList.Domain;
using System.Text.Json;
using Microsoft.Extensions.Options;

public class FileTareasRepository : ITareasRepository
{
    private readonly string _path;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public FileTareasRepository(IOptions<FileSettings> settings)
    {
        _path = settings.Value.Path;
    }

    public async Task AddAsync(Tarea tarea)
    {
        await _lock.WaitAsync();
        try
        {
            var tareas = await LeerTareasAsync();

            tareas.Add(tarea);

            await GuardarTareasAsync(tareas);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task UpdateAsync(Tarea tarea)
    {
        await _lock.WaitAsync();
        try
        {
            var listTareas = await LeerTareasAsync();

            var indexTarea = listTareas.FindIndex(t => t.Id == tarea.Id);

            if (indexTarea == -1)
                throw new KeyNotFoundException("No se encontró la tarea.");

            listTareas[indexTarea] = tarea;

            await GuardarTareasAsync(listTareas);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Tarea?> GetByIdAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            var tareas = await LeerTareasAsync();

            return tareas.FirstOrDefault(t => t.Id == id);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<Tarea>> GetAllAsync()
    {
        await _lock.WaitAsync();
        try
        {
            return await LeerTareasAsync();
        }
        finally
        {
            _lock.Release();
        }
    }
    public async Task<bool> ExistsByNameAsync(string nombre)
    {
                await _lock.WaitAsync();
                nombre = nombre.ToLower();
        try
        {
            var tareas = await LeerTareasAsync();
            
            return tareas.Exists(n=> n.Nombre.ToLower() == nombre) ? true : false;

        }
        finally
        {
            _lock.Release();
        }
    }
    public async Task DeleteAsync(Guid id)
    {
        await _lock.WaitAsync();
        try
        {
            var tareas = await LeerTareasAsync();

            var eliminadas = tareas.RemoveAll(t => t.Id == id);

            if (eliminadas == 0)
                throw new KeyNotFoundException("No se encontró la tarea.");

            await GuardarTareasAsync(tareas);
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<List<Tarea>> LeerTareasAsync()
    {
        if (!File.Exists(_path))
            return new List<Tarea>();

        var json = await File.ReadAllTextAsync(_path);

        if (string.IsNullOrWhiteSpace(json))
            return new List<Tarea>();

        return JsonSerializer.Deserialize<List<Tarea>>(json)?? new List<Tarea>();
    }

    private async Task GuardarTareasAsync(List<Tarea> tareas)
    {
        var json = JsonSerializer.Serialize(
            tareas,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        await File.WriteAllTextAsync(_path, json);
    }
}