using FluentValidation;
using FluentValidation.Results;
using MeuLivroReceitas.Comunication.Request;
using MeuLivroReceitas.Domain.Extension;
using MeuLivroReceitas.Exceptions;

namespace MeuLivroReceitas.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRegisterRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(c => c.Title).NotEmpty().WithMessage(ResourceMessageError.EMPTY_USER);
        RuleFor(c => c.Category).IsInEnum().WithMessage(ResourceMessageError.EMAIL_EMPTY);
        RuleFor(c => c.MethodPreparation).NotEmpty().WithMessage(ResourceMessageError.EMAIL_EMPTY);
        RuleFor(c => c.PreparationTime).InclusiveBetween(1, 1000).WithMessage(ResourceMessageError.PREPARATION_TIME_INVALID);
        RuleFor(c => c.Ingredients).NotEmpty().WithMessage(ResourceMessageError.PHONE_EMPTY);

        //validacao para cada elemento dentro da lista de ingredientes
        RuleForEach(c => c.Ingredients).ChildRules(Ingredient =>
        {
            Ingredient.RuleFor(i => i.Product).NotEmpty();
            Ingredient.RuleFor(i => i.Quantity).NotEmpty();
        });

        //nao criar um ingrediente que ja existe
        RuleFor(c => c.Ingredients).Custom((ingredients, context) =>
        {
            //dentro da lista de productsDistint tera somente strings que nao sao iguais
            //RemoveAccents criado em domain.extension
            var productsDistint = ingredients.Select(i => i.Product.RemoveAccents().ToLower()).Distinct();
            //se o tamanho de productsDistint
            // for diferent do tamanho da lista de ingredients
            //significa que na lista contem produtos repetidos
            if (productsDistint.Count() != ingredients.Count())
            {
                //adiciona falha ao context
                context.AddFailure(new ValidationFailure("Ingredients", ""));
            }
        });
    }
}
