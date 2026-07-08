
using Microsoft.Extensions.Configuration;

namespace todoList.Application.Services;

public class JwtService
{
    private readonly string _secretKey;

    public JwtService(IConfiguration configuration)

    {
        _secretKey = configuration["Jwt:SecretKey"];    
    }
    
}
