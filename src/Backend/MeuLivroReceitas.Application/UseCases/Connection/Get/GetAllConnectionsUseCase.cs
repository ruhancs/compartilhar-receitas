using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Repositories.Connection;
using MeuLivroReceitas.Domain.Repositories.Recipe;

namespace MeuLivroReceitas.Application.UseCases.Connection.Get;

public class GetAllConnectionsUseCase : IGetAllConnectionsUseCase
{
    private readonly IAuthenticatedUser _authenticatedUser;
    private readonly IConnectionReadOnlyRepository _connectionRepository;
    private readonly IMapper _mapper;
    private readonly IRecipeReadOnlyRepository _recipeRepository;

    public GetAllConnectionsUseCase(
        IAuthenticatedUser authenticatedUser,
        IConnectionReadOnlyRepository connectionRepository,
        IMapper mapper,
        IRecipeReadOnlyRepository recipeRepository
        )
    {
        _authenticatedUser = authenticatedUser;
        _connectionRepository = connectionRepository;
        _mapper = mapper;
        _recipeRepository = recipeRepository;
    }

    public async Task<IList<ResponseUserConnectedJson>> Execute()
    {
        var user = await _authenticatedUser.GetUser();

        var allConnetions = await _connectionRepository.GetUserConnetions(user.Id);

        //lista de tarefas
        //iterar em cada usuario da lista de allConnetions
        var tasks = allConnetions.Select(async userConnected =>
        {
            //quantidade de receitas dos usuarios das connectionsResponse
            //inserir quantidades de receitas da conexao
            var quantityRecipes = await _recipeRepository.QuantityRecipes(userConnected.Id);

            //transforma userConnected criada em GetUserConnetions
            //para ResponseUserConnectedJson
            //configurar em automapperConfig
            var userJson = _mapper.Map<ResponseUserConnectedJson>(userConnected);

            //adcionar quantidade de receitas de cada usuario
            userJson.QuantityRecipes = quantityRecipes;

            return userJson;
        });

        return await Task.WhenAll(tasks);
    }
}
