using MeuLivroReceitas.Application.UseCases.Login.DoLogin;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroReceitas.Api.Controllers;


public class LoginController : MeuLivroReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseLoginJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUseCase useCase,
        [FromBody] RequestLoginJson req
        )
    {
        var response = await useCase.Execute(req);

        return Ok(response);
    }
}
