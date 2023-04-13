using FluentValidation;
using MeuLivroReceitas.Comunication.Request;

namespace MeuLivroReceitas.Application.UseCases.Recipe.Update;

public class UpdateRecipeValidator : AbstractValidator<RequestRegisterRecipeJson>
{
    public UpdateRecipeValidator()
    {
        //utilizar o validator criado em RecipeValidator
        RuleFor(r => r).SetValidator(new RecipeValidator());
    }
}
