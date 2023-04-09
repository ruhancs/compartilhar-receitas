using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroReceitas.Api.Controllers
{

    public class UserController : MeuLivroReceitasController
    {

        [HttpPost]
        //informar no swagger qual o codigo que ira retorna e o corpo que ira retornar
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase usecase,
            [FromBody] RequestRegisterUserJson req
            )
        {
            var result = await usecase.Execute(req);

            return Created(string.Empty, result);
        }
    }
}