namespace todoList.Infrastructure;
using todoList.Application.Services;
public class HttpCatService : IHttpCatService
{
    private readonly HttpClient _httpClient;
    public HttpCatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
   public async Task<string> GetImageUrlAsync(int statusCode)
    {
        string catUrl = $"https://http.cat/{statusCode}";

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Head, catUrl);
            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode ? catUrl: null;
        }
        catch (HttpRequestException)
        {
            
            return null;
        }
    }

}
