using MeuLivroReceitas.Api.Filters;
using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Application.UseCases.User.Update;
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
        
        [HttpPut]
        [Route("change-password")]
        //ServiceFilter para injetar as dependencias de AuthenticatedUserAttr
        [ServiceFilter(typeof(AuthenticatedUserAttr))]//indica que o endpoint esta protegido por authenticacao
        //informar no swagger qual o codigo que ira retorna e o corpo que ira retornar
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePassword(
            [FromServices] IUpdatePasswordUseCase usecase,
            [FromBody] RequestUpdatePasswordJson req
            )
        {
            await usecase.Execute(req);

            return NoContent();
        }
    }
}