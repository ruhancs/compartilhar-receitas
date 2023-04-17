using MeuLivroReceitas.Api.Binder;
using MeuLivroReceitas.Api.Filters.AuthenticatedUser;
using MeuLivroReceitas.Application.UseCases.Connection.Get;
using MeuLivroReceitas.Application.UseCases.Connection.Remove;
using MeuLivroReceitas.Comunication.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroReceitas.Api.Controllers;

public class ConnectionsController : MeuLivroReceitasController
{
    [HttpGet]
    //ServiceFilter para injetar as dependencias de AuthenticatedUserAttr
    [ServiceFilter(typeof(AuthenticatedUserAttr))]//indica que o endpoint esta protegido por authenticacao
    //informar no swagger qual o codigo que ira retorna e o corpo que ira retornar
    [ProducesResponseType(typeof(List<ResponseUserConnectedJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]// se o usuario na tiver receitas retorna 204
    public async Task<IActionResult> GetConnections(
        [FromServices] IGetAllConnectionsUseCase usecase
        )
    {
        var results = await usecase.Execute();

        if (results.Any())
        {
            return Ok(results);
        }

        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id:hashids}")]
    //ServiceFilter para injetar as dependencias de AuthenticatedUserAttr
    [ServiceFilter(typeof(AuthenticatedUserAttr))]//indica que o endpoint esta protegido por authenticacao
    //informar no swagger qual o codigo que ira retorna e o corpo que ira retornar
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveConnection(
        [FromServices] IRemoveConnectionUseCase usecase,
        //HashIdModelBinder criado em Binder
        //para transformar id de string para numero
        //decodifica o id
        //em filters/Swagger criado HashIdOperationFilter
        //e configurado em Program o Services.AddSwaggerGe para receber string como parametro em id
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id // pegar da rota o id
        )
    {
        await usecase.Execute(id);

        return NoContent();
    }
}
