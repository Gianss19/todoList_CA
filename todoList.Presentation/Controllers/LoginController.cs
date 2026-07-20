using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todoList.Application.DTO.Login;
using todoList.Application.UseCases.Login;

namespace todoList.Api.Controllers;

[ApiController]
[Route("api/login")]
[Produces("application/json")]
public class LoginController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    public LoginController(LoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if(request == null)
            return Unauthorized();

        var token = await _loginUseCase.Login(request);
        return Content(token, "application/json");
    }


}
