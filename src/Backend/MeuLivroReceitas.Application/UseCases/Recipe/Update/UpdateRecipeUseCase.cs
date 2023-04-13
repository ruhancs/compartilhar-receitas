using AutoMapper;
using MeuLivroReceitas.Application.Services.AuthUser;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using MeuLivroReceitas.Exceptions.ExceptionsBase;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Domain.Entities;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Application.UseCases.Recipe.Register;

namespace MeuLivroReceitas.Application.UseCases.Recipe.Update;

//para utilizala adicionar em Bootstraped em addUseCase
//para injecao de dependencia
public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly IAuthenticatedUser _user;

    //UnitOfWork para fazer o commit e salvar no db
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public UpdateRecipeUseCase(
        IRecipeUpdateOnlyRepository repository,
        IAuthenticatedUser user,
        IMapper mapper,
        IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        _user = user;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long recipeId,RequestRegisterRecipeJson req)
    {
        //GetUser implementado em Application services authenticatedUser
        //pega o usuario utilizando o token do header
        var user = await _user.GetUser();

        var recipe = await _repository.GetRecipesByIdForUpdate(recipeId);

        Validate(user, recipe, req);

        //transforma RequestRegisterRecipeJson em Recipe
        //configurado em services/automapper/automapperConfig em RequestEntity
        //reaproveita recipe criado
        _mapper.Map(req, recipe);

        _repository.Update(recipe);

        await _unitOfWork.Commit();
    }

    public void Validate(
    Domain.Entities.Usuario user,
    Domain.Entities.Recipe recipe,
    Comunication.Request.RequestRegisterRecipeJson req
    )
    {
        if (recipe is null || recipe.UserId != user.Id)
        {
            throw new ValidationErrors(new List<string> { ResourceMessageError.RECIPE_NOT_FOUND });
        }

        var validator = new UpdateRecipeValidator();
        //resultado das validacoes de req
        var result = validator.Validate(req);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(c => c.ErrorMessage).ToList();
            throw new ValidationErrors(errorMessages);
        }
    }
}
