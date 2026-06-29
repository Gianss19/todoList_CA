namespace todoList.Application;

public interface IHttpCatService
{
 public Task<string?> GetImageUrlAsync(int statusCode);
}
