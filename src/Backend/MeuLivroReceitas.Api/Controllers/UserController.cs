using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroReceitas.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        [HttpPost(Name = "RegisterUser")]
        //informar no swagger qual o codigo que ira retorna e o corpo que ira retornar
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterUser(
            [FromServices] IRegisterUserUseCase usecase,
            [FromBody] RequestRegisterUserJson req
            )
        {
            var result = await usecase.Execute(req);

            return Created(string.Empty, result);
        }
    }
}