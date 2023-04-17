using MeuLivroReceitas.Api.Binder;
using MeuLivroReceitas.Api.Filters.AuthenticatedUser;
using MeuLivroReceitas.Application.UseCases.Recipe.Delete;
using MeuLivroReceitas.Application.UseCases.Recipe.GetById;
using MeuLivroReceitas.Application.UseCases.Recipe.Register;
using MeuLivroReceitas.Application.UseCases.Recipe.Update;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroReceitas.Api.Controllers;

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
    
    [HttpGet]
    [Route("{id:hashids}")]
    //tipo de resposta que produz ResponseRecipeJson status 201
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        [FromServices]IGetRecipeByIdUseCase useCase,
        //HashIdModelBinder criado em Binder
        //para transformar id de string para numero
        //decodifica o id
        //em filters/Swagger criado HashIdOperationFilter
        //e configurado em Program o Services.AddSwaggerGe para receber string como parametro em id
        [FromRoute] [ModelBinder(typeof(HashidsModelBinder))] long id // pegar da rota o id
        )
    {
        //var recipeId = Convert.ToInt64(id);

        var response = await useCase.Execute(id);

        return Ok(response);
    }
    
    [HttpPut]
    [Route("{id:hashids}")]
    //tipo de resposta que produz ResponseRecipeJson status 201
    [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        [FromServices]IUpdateRecipeUseCase useCase,
        [FromBody]RequestRegisterRecipeJson req,
        //HashIdModelBinder criado em Binder
        //para transformar id de string para numero
        //decodifica o id
        //em filters/Swagger criado HashIdOperationFilter
        //e configurado em Program o Services.AddSwaggerGe para receber string como parametro em id
        [FromRoute] [ModelBinder(typeof(HashidsModelBinder))] long id // pegar da rota o id
        )
    {
        //var recipeId = Convert.ToInt64(id);

        await useCase.Execute(id,req);

        return NoContent();
    }
    
    [HttpDelete]
    [Route("{id:hashids}")]
    //tipo de resposta que produz ResponseRecipeJson status 201
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        [FromServices]IDeleteRecipeUseCase useCase,
        //HashIdModelBinder criado em Binder
        //para transformar id de string para numero
        //decodifica o id
        //em filters/Swagger criado HashIdOperationFilter
        //e configurado em Program o Services.AddSwaggerGe para receber string como parametro em id
        [FromRoute] [ModelBinder(typeof(HashidsModelBinder))] long id // pegar da rota o id
        )
    {
        //var recipeId = Convert.ToInt64(id);

        await useCase.Execute(id);

        return NoContent();
    }
}
