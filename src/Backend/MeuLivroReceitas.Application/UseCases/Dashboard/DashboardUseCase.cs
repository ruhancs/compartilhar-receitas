using System.Globalization;
using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Comunication.Response;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Extension;
using MeuLivroReceitas.Domain.Repositories.Connection;
using MeuLivroReceitas.Domain.Repositories.Recipe;

namespace MeuLivroReceitas.Application.UseCases.Dashboard;

//para utilizala adicionar em Bootstraped em addUseCase
//para injecao de dependencia
public class DashboardUseCase : IDashboardUseCase
{
    private readonly IRecipeReadOnlyRepository _repository;
    private readonly IAuthenticatedUser _user;

    //para converter a Request em uma entidade de receita para salvar no db
    //e converter a entidade na resposta
    //configurar o _mapper em services automapper
    //automapperConfig em EntityResponse
    private readonly IMapper _mapper;

    private readonly IConnectionReadOnlyRepository _connectionRepository;

    public DashboardUseCase(
        IRecipeReadOnlyRepository repository,
        IAuthenticatedUser user,
        IMapper mapper,
        IConnectionReadOnlyRepository connectionRepository
        )
    {
        _repository = repository;
        _user = user;
        _mapper = mapper;
        _connectionRepository = connectionRepository;
    }
    public async Task<ResponseDashboardJson> Execute(RequestDashboardJson req)
    {
        //GetUser implementado em Application services authenticatedUser
        //pega o usuario utilizando o token do header
        var user = await _user.GetUser();

        var recipes = await _repository.GetAllRecipesUser(user.Id);

        recipes = Filter(req, recipes );

        var recipesUsersConnected = await RecipesConnectedUsers(req, user);

        //concatenar as receitas do usuario com as do usuarios conectados
        recipes = recipes.Concat(recipesUsersConnected).ToList();

        return new ResponseDashboardJson
        {
            //transforma recipes em List<ResponseRecipeDashboardJson>
            Recipes = _mapper.Map<List<ResponseRecipeDashboardJson>>(recipes)
        };
    }

    private async Task<IList<Domain.Entities.Recipe>> RecipesConnectedUsers(
        RequestDashboardJson req,
        Usuario user
        )
    {
        var allConnetions = await _connectionRepository.GetUserConnetions(user.Id);

        //pegar o id dos usuarios conectados
        var usersConnectedIds = allConnetions.Select(c => c.Id).ToList();

        var recipesUsersConnected = await _repository.GetAllRecipesConnectedUsers(usersConnectedIds);

        return Filter(req, recipesUsersConnected);


    }

    //filtro das receitas
    private static IList<Domain.Entities.Recipe> Filter(RequestDashboardJson req, IList<Domain.Entities.Recipe> recipes)
    {
        if(recipes is null)
        {
            return new List<Domain.Entities.Recipe>();
        }

        var filteredRecipes = recipes;

        if (req.Category.HasValue)
        {
            //(Domain.Enum.Category) req.Category.Value tranforma para o mesmo tipo de r.Category
            //que é um enum das categorias
            filteredRecipes = recipes.Where(r => r.Category == (Domain.Enum.Category)req.Category.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(req.TypeOrIngredient))
        {
            //WordCompareInsensitive criado em domain.extension

            filteredRecipes = recipes.Where(r => 
                r.Title.WordCompareInsensitive(req.TypeOrIngredient) ||
                r.Ingredients.Any(i => i.Product.WordCompareInsensitive(req.TypeOrIngredient))
                ).ToList();
        }

        return filteredRecipes.OrderBy(r => r.Title).ToList();
    }
}
