using FluentValidation;
using MeuLivroReceitas.Comunication.Request;

namespace MeuLivroReceitas.Application.UseCases.Recipe.Register;

//validacoes do request para validar receitas
public class RegisterRecipeValidator : AbstractValidator<RequestRegisterRecipeJson>
{
    public RegisterRecipeValidator()
    {
        //utilizar o validator criado em RecipeValidator
        RuleFor(r => r).SetValidator(new RecipeValidator());
    }
}
