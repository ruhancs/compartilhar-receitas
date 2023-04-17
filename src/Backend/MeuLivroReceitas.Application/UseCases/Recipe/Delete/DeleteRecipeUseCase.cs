using MeuLivroReceitas.Application.Services.AuthenticatedUser;
using MeuLivroReceitas.Domain.Repositories;
using MeuLivroReceitas.Domain.Repositories.Recipe;
using MeuLivroReceitas.Exceptions;
using MeuLivroReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroReceitas.Application.UseCases.Recipe.Delete
{
    public class DeleteRecipeUseCase : IDeleteRecipeUseCase
    {
        private readonly IRecipeWriteOnlyRepository _writeOnlyRepository;
        private readonly IRecipeReadOnlyRepository _readOnlyRepository;
        private readonly IAuthenticatedUser _user;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRecipeUseCase(
            IRecipeWriteOnlyRepository writeOnlyRepository,
            IRecipeReadOnlyRepository readOnlyRepository,
            IAuthenticatedUser user,
            IUnitOfWork unitOfWork
            )
        {
            _writeOnlyRepository = writeOnlyRepository;
            _readOnlyRepository = readOnlyRepository;
            _user = user;
            _unitOfWork = unitOfWork;
        }

        //para utilizala adicionar em Bootstraped em addUseCase
        //para injecao de dependencia
        public async Task Execute(long recipeId)
        {
            //GetUser implementado em Application services authenticatedUser
            //pega o usuario utilizando o token do header
            var user = await _user.GetUser();

            var recipe = await _readOnlyRepository.GetRecipesById(recipeId);

            Validate(user, recipe);

            await _writeOnlyRepository.Delete(recipeId);

            await _unitOfWork.Commit();
        }

        public void Validate(
        Domain.Entities.Usuario user,
        Domain.Entities.Recipe recipe
        )
        {

            if (recipe is null || recipe.UserId != user.Id)
            {
                throw new ValidationErrors(new List<string> { ResourceMessageError.RECIPE_NOT_FOUND });
            }

        }
    }
}
