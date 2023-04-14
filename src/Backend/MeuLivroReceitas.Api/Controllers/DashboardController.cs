using MeuLivroReceitas.Api.Filters.AuthenticatedUser;
using MeuLivroReceitas.Application.UseCases.Dashboard;
using MeuLivroReceitas.Application.UseCases.User.Register;
using MeuLivroReceitas.Application.UseCases.User.Update;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroReceitas.Api.Controllers
{

    public class DashboardController : MeuLivroReceitasController
    {       
        //Put pois sera enviado no corpo os filtros
        //opcao seria mandar na rota
        [HttpPut]
        //ServiceFilter para injetar as dependencias de AuthenticatedUserAttr
        [ServiceFilter(typeof(AuthenticatedUserAttr))]//indica que o endpoint esta protegido por authenticacao
        //informar no swagger qual o codigo que ira retorna e o corpo que ira retornar
        [ProducesResponseType(typeof(ResponseDashboardJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]// se o usuario na tiver receitas retorna 204
        public async Task<IActionResult> GetDashboard(
            [FromServices] IDashboardUseCase usecase,
            [FromBody] RequestDashboardJson req
            )
        {
            var result = await usecase.Execute(req);

            if (result.Recipes.Any())
            {
                return Ok(result);
            }

            return NoContent();
        }
    }
}