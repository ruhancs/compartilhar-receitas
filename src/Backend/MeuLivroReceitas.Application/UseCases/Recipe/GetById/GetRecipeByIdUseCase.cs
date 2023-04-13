using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthUser;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroReceitas.Application.UseCases.Recipe.GetById;

//para utilizala adicionar em Bootstraped em addUseCase
//para injecao de dependencia
public class GetRecipeByIdUseCase : IGetRecipeByIdUseCase
{
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IAuthenticatedUser _user;

    //para converter a Request em uma entidade de receita para salvar no db
    //e converter a entidade na resposta
    //configurar o _mapper em services automapper
    //automapperConfig em EntityResponse
    private readonly IMapper _mapper;

    public GetRecipeByIdUseCase(
        IRecipeReadOnlyRepository repository,
        IAuthenticatedUser user,
        IMapper mapper
        )
    {
        _repository = repository;
        _user = user;
        _mapper = mapper;
    }

    public async Task<ResponseRecipeJson> Execute(long recipeId)
    {
        //GetUser implementado em Application services authenticatedUser
        //pega o usuario utilizando o token do header
        var user = await _user.GetUser();

        var recipe = await _repository.GetRecipesById( recipeId );

        Validate(user,recipe);

        //transforma recipe em ResponseRecipeJson
        return _mapper.Map<ResponseRecipeJson>(recipe);
    }

    public void Validate(
        Domain.Entities.Usuario user,
        Domain.Entities.Recipe recipe)
    {
        if ( recipe is null || recipe.UserId != user.Id )
        {
            throw new ValidationErrors(new List<string> { ResourceMessageError.RECIPE_NOT_FOUND });
        }
    }
}
