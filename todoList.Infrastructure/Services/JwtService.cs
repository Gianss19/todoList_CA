using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using todoList.Application.Services;

namespace todoList.Infrastructure.Services;

public class JwtService : ITokenService
{
    private readonly IConfiguration _configuration;
    

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(Guid id, string nombre, string correo, string rol)
    {
        rol = rol.ToLower();
        var secretKey = _configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("El SecretKey no está configurado."); // Se buscan los parametros para firmar token
        var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("El Issuer no está configurado.");
        var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("El Audience no está configurado.");
        
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Se crea la clave de seguridad para firmar el token(forzosamente en matriz de bytes, NO string)
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // Se crean las credenciales que se usaran para firmar el token
        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, nombre));
        claims.Add(new Claim(ClaimTypes.Email, correo));
        claims.Add(new Claim(ClaimTypes.Role, rol));

        var descriptor = new SecurityTokenDescriptor // el empaquetador para crear el token
        {
            Subject = new ClaimsIdentity(claims), //el sujeto, la persona que será autenticada, Identity es el agrupamiento de claims en un objeto 
            Expires = DateTime.UtcNow.AddHours(2),  //la fecha y hora hasta la que se validará el token, por defecto es 1h
            Issuer = issuer, //el emisor del token, puede ser cualquier string
            Audience = audience, //el destinatario del token, puede ser cualquier string
            SigningCredentials = credentials // las credenciales para firmar el token
        };
        var tokenHandler = new JwtSecurityTokenHandler(); // la clase que se encarga de crear y validar los tokens JWT
        
        var token = tokenHandler.CreateToken(descriptor);
        
        return tokenHandler.WriteToken(token); // el método WriteToken convierte el token en un string
    }
}

          



        




