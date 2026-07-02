namespace todoList.Application.Services;

public interface IHttpCatService
{
 public Task<string?> GetImageUrlAsync(int statusCode);
}
