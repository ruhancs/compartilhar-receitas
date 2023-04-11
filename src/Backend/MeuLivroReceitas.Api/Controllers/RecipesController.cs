using MeuLivroReceitas.Api.Filters;
using MeuLivroReceitas.Application.UseCases.Recipe;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroReceitas.Api.Controllers
{
    //toda classe precisa estar logado
    [ServiceFilter(typeof(AuthenticatedUserAttr))]//indica que o endpoint esta protegido por authenticacao
    public class RecipesController : MeuLivroReceitasController
    {
        [HttpPost]
        //tipo de resposta que produz ResponseRecipeJson status 201
        [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(
            [FromServices]IRegisterRecipeUseCase useCase,
            [FromBody]RequestRegisterRecipeJson req
            )
        {
            var response = await useCase.Execute(req);

            return Created(string.Empty, response);
        }
    }
}
